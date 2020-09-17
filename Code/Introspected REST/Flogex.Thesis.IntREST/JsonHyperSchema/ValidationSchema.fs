module internal Flogex.Thesis.JsonHyperSchema.ValidationSchema

open System
open Flogex.Thesis.JsonHyperSchema.Attributes
open Flogex.Thesis.JsonHyperSchema.JsonTypes
open Flogex.Thesis.JsonHyperSchema.JsonTypes.Restrictions
open Flogex.Thesis.JsonHyperSchema.Primitives
open Flogex.Thesis.JsonHyperSchema.Reflection

// Use only when AllowMultiple is false for all attributes you are interested in
let private toSingleAttributesMap map =
    Map.map (fun _ attrs -> Array.head attrs) map

let private toModelProperty (prop: Reflection.PropertyInfo) =
    let attributes =
        getAttributes prop
        |> Seq.groupBy (fun attr -> (attr.GetType().FullName))
        |> Seq.map (fun (name, attrs) -> (name, Seq.toArray attrs))
        |> Map.ofSeq

    { Name = prop.Name; Type = prop.PropertyType; Attributes = attributes }

let private isNonNullable prop =
    AttributesMap.contains<NonNullableAttribute> prop.Attributes

let private addNonNullability prop = function
    | Nullable t when isNonNullable prop -> t
    | _ as t -> t

let rec private addRestrictions prop = function
    | JsonNumber r ->
        let attrs = toSingleAttributesMap prop.Attributes
        JsonNumber <| NumberRestrictions.fromAttributes r attrs
    | JsonString r ->
        let attrs = toSingleAttributesMap prop.Attributes
        JsonString <| StringRestrictions.fromAttributes r attrs
    | JsonArray (itemsType, r) ->
        let attrs = toSingleAttributesMap prop.Attributes
        JsonArray (itemsType, ArrayRestrictions.fromAttributes r attrs)
    | Nullable t -> Nullable <| addRestrictions prop t
    | _ as t -> t

let private getEnumJsonType (t: Type) =
    let values = t.GetEnumNames()
    JsonString { StringRestrictions.None with Enum = values }

let private dateStringRestrictions = { StringRestrictions.None with Format = Some "date-time" }

let private getJsonProperties getJsonType getLinks t =
    getPublicProperties t
    |> Seq.map toModelProperty
    |> Seq.map (fun p -> {
           Name = p.Name
           Schema = {
               Type = getJsonType p.Type |> addNonNullability p |> addRestrictions p
               Links = getLinks p
           }
           Required = AttributesMap.contains<RequiredAttribute> p.Attributes
       })

let rec getJsonValueType (getLinks: ModelProperty -> seq<LinkDescriptor>) (clrType: Type): JsonType =
    let getJsonType' = getJsonType getLinks
    let getJsonProperties' = getJsonProperties getJsonType' getLinks

    match clrType with
    | Bool -> JsonBoolean
    | Int -> JsonInteger NumberRestrictions.None
    | Float -> JsonNumber NumberRestrictions.None
    | String -> JsonString StringRestrictions.None
    | Enumerable ->
        let itemsType = Option.get <| getEnumerableType clrType
        let schema = {
            Type = getJsonType' itemsType
            Links = getLinks <| ModelProperty.ofType itemsType
        }
        JsonArray (schema, ArrayRestrictions.None)
    | Date -> JsonString dateStringRestrictions
    | Object -> JsonObject (getJsonProperties' clrType)

and getJsonType (getLinks: ModelProperty -> seq<LinkDescriptor>) (clrType: Type): JsonType =
    let getJsonValueType' = getJsonValueType getLinks

    match clrType with
    | NullableType t -> Nullable (getJsonValueType' t)
    | Enum -> getEnumJsonType clrType
    | ValueType -> getJsonValueType' clrType
    | Class -> Nullable (getJsonValueType' clrType)