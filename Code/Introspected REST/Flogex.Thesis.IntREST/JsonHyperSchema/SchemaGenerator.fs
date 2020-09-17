[<RequireQualifiedAccess>]
module Flogex.Thesis.JsonHyperSchema.SchemaGenerator

open System
open Flogex.Thesis.JsonHyperSchema.Primitives
open Flogex.Thesis.JsonHyperSchema.ValidationSchema
open Flogex.Thesis.JsonHyperSchema.JsonTypes

let rec getInputSchema t = { Type = getJsonType' t }

and private getLinks = Links.forProperty getInputSchema >> Seq.ofArray

and private getJsonType' = getJsonType getLinks

let private getJsonValueType' = getJsonValueType getLinks

[<CompiledName("GenerateFromType")>]
let fromType (clrType: Type) : JsonSchema =
    { Type = getJsonValueType' clrType
      Links = getLinks <| ModelProperty.ofType clrType }

[<CompiledName("GenerateFromType")>]
let fromType'<'T> : JsonSchema = fromType typeof<'T>