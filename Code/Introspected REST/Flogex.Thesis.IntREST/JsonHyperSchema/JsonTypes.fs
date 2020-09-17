namespace Flogex.Thesis.JsonHyperSchema.JsonTypes

open System.Text.RegularExpressions
open Tavis.UriTemplates
open Flogex.Thesis.JsonHyperSchema.Primitives

type RangeLimit =
    | InclusiveLimit of double
    | ExclusiveLimit of double

type NumberRestrictions =
    { MultipleOf: PositiveInt option
      Minimum: RangeLimit option
      Maximum: RangeLimit option }

type StringRestrictions =
    { MinLength: NonNegativeInt option
      MaxLength: NonNegativeInt option
      Pattern: Regex option
      Format: string option
      Enum: string[] }

type ArrayRestrictions =
    { MinItems: NonNegativeInt option
      MaxItems: NonNegativeInt option
      UniqueItems: bool option }

type JsonType =
    | JsonBoolean
    | JsonInteger of restrictions: NumberRestrictions
    | JsonNumber of restrictions: NumberRestrictions
    | JsonString of restrictions: StringRestrictions
    | JsonArray of items: JsonSchema * restrictions: ArrayRestrictions
    | JsonObject of properties: seq<JsonProperty>
    | Nullable of underlyingType: JsonType

and JsonProperty =
    { Schema: JsonSchema
      Name: string
      Required: bool }
    //TODO title, description, default, deprecated, readOnly, writeOnly

and LinkDescriptor =
    { Rel: string
      Href: UriTemplate
      TemplateRequired: string[]
      TemplatePointers: Map<string, string>
      HrefSchema: InputJsonSchema option
      TargetHints: Map<string, string[]>
      TargetMediaType: string option }

and InputJsonSchema = { Type: JsonType }

and JsonSchema =
    { Type: JsonType
      Links: seq<LinkDescriptor> }