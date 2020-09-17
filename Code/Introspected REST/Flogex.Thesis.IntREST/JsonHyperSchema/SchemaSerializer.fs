module Flogex.Thesis.JsonHyperSchema.SchemaSerializer

open System.Text.Json
open Flogex.Thesis.JsonHyperSchema.JsonTypes
open Flogex.Thesis.JsonHyperSchema.JsonWriter

// Helper functions

let private writeNothing = id

let private writeIfSome f x  =
    if Option.isSome x then f (Option.get x) else writeNothing

let private writeIfAny f seq =
    if Seq.isEmpty seq then writeNothing else f seq

let private flip f a b = f b a

let private writeSequence f xs writer =
    Seq.fold (flip f) writer xs

let private writeArray f arr =
    writeStartArray
    >> writeSequence f arr
    >> writeEndArray

let private writeMap f m =
    let writeEntry (k, v) = writePropertyName k >> f v

    writeStartObject
    >> writeSequence writeEntry (Map.toSeq m)
    >> writeEndObject

// Links

let private writeTemplatePointer (variable, pointer) =
    writeString variable pointer

let private writeTemplatePointers tps =
    writePropertyName "templatePointers"
    >> writeStartObject
    >> writeSequence writeTemplatePointer tps
    >> writeEndObject

let private writeTemplateRequired tr =
    writePropertyName "templateRequired"
    >> writeStartArray
    >> writeSequence writeStringValue tr
    >> writeEndArray

// Json Types

let private writeType t =
    let rec writeTypeName = function
        | JsonBoolean -> writeStringValue "boolean"
        | JsonInteger _ -> writeStringValue "integer"
        | JsonNumber _ -> writeStringValue "number"
        | JsonString _ -> writeStringValue "string"
        | JsonArray _ -> writeStringValue "array"
        | JsonObject _ -> writeStringValue "object"
        | Nullable t ->
            writeStartArray
            >> writeStringValue "null"
            >> writeTypeName t
            >> writeEndArray

    writePropertyName "type" >> writeTypeName t

// Restrictions

let private writeNumberRestrictions r =
    writeIfSome (writePositiveInt "multipleOf") r.MultipleOf
    >> writeIfSome (fun limit ->
         match limit with
         | InclusiveLimit value -> writeDouble "minimum" value
         | ExclusiveLimit value -> writeDouble "exclusiveMinimum" value) r.Minimum
    >> writeIfSome (fun limit ->
         match limit with
         | InclusiveLimit value -> writeDouble "maximum" value
         | ExclusiveLimit value -> writeDouble "exclusiveMaximum" value) r.Maximum

let private writerStringRestrictions r =
    let writeEnumRestriction enum =
        writePropertyName "enum"
        >> writeStartArray
        >> writeSequence writeStringValue enum
        >> writeEndArray

    writeIfSome (writeNonNegativeInt "minLength") r.MinLength
    >> writeIfSome (writeNonNegativeInt "maxLength") r.MaxLength
    >> writeIfSome (fun regex -> writeString "pattern" (regex.ToString())) r.Pattern
    >> writeIfSome (writeString "format") r.Format
    >> writeIfAny writeEnumRestriction r.Enum

let private writeTargetHints (h: Map<string, string[]>) =
    writePropertyName "targetHints" >> writeMap (writeArray writeStringValue) h

let rec private writeArrayRestrictions (s, r) =
    let writeItemsRestrictions s =
        writePropertyName "items"
        >> writeStartObject
        >> writeSchema s
        >> writeEndObject

    writeIfSome (writeNonNegativeInt "minItems") r.MinItems
    >> writeIfSome (writeNonNegativeInt "maxItems") r.MaxItems
    >> writeIfSome (writeBoolean "uniqueItems") r.UniqueItems
    >> writeItemsRestrictions s

and private writeObjectProperties ps =
    let writeProperty p =
        writePropertyName p.Name
        >> writeStartObject
        >> writeSchema p.Schema
        >> writeEndObject

    let writeRequiredProperties ps =
        let requiredProps = Seq.filter (fun p -> p.Required) ps

        if Seq.isEmpty requiredProps
        then writeNothing
        else
            writePropertyName("required")
            >> writeStartArray
            >> writeSequence (fun p -> writeStringValue (p.Name)) requiredProps
            >> writeEndArray

    writePropertyName "properties"
    >> writeStartObject
    >> writeSequence writeProperty ps
    >> writeEndObject
    >> writeRequiredProperties ps

and private writeRestrictions = function
    | JsonBoolean -> writeNothing
    | JsonInteger r | JsonNumber r -> writeNumberRestrictions r
    | JsonString r -> writerStringRestrictions r
    | JsonType.JsonArray (s, r) -> writeArrayRestrictions (s, r)
    | JsonObject ps -> writeObjectProperties ps
    | Nullable t -> writeRestrictions t

and private writeHrefSchema (is: InputJsonSchema) =
    writePropertyName ("hrefSchema")
    >> writeStartObject
    >> writeSchema { Type = is.Type; Links = Seq.empty<LinkDescriptor> }
    >> writeEndObject

and private writeLink l =
    writeStartObject
    >> writeString "rel" l.Rel
    >> writeString "href" (l.Href.ToString())
    >> writeIfAny writeTemplateRequired l.TemplateRequired
    >> writeIfAny writeTemplatePointers (Map.toSeq l.TemplatePointers)
    >> writeIfSome writeHrefSchema l.HrefSchema
    >> writeIfAny writeTargetHints l.TargetHints
    >> writeIfSome (writeString "targetMediaType") l.TargetMediaType
    >> writeEndObject

and writeLinks ls =
    writePropertyName "links"
    >> writeStartArray
    >> writeSequence writeLink ls
    >> writeEndArray

and private writeSchema (s: JsonSchema) =
    writeType s.Type >> writeRestrictions s.Type >> writeIfAny writeLinks s.Links

// Public interface

[<CompiledName("Serialize")>]
let serialize (schema: JsonSchema) (jsonWriter: Utf8JsonWriter) (options: JsonSerializerOptions): unit =
    (jsonWriter, options)
    |> writeStartObject
    |> writeString "$schema" "https://json-schema.org/draft/2019-09/hyper-schema"
    |> writeSchema schema
    |> writeEndObject
    |> ignore
