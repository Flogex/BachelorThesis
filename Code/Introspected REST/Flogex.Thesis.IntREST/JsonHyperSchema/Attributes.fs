namespace Flogex.Thesis.JsonHyperSchema.Attributes

open System
open System.Runtime.InteropServices

// Common property attributes

[<AttributeUsage(AttributeTargets.Property)>]
type NonNullableAttribute () = inherit Attribute()

[<AttributeUsage(AttributeTargets.Property)>]
type RequiredAttribute () = inherit Attribute()

// Number

[<AttributeUsage(AttributeTargets.Property)>]
type MultipleOfAttribute (multipleOf: int) =
    inherit Attribute()
    member val MultipleOf = multipleOf

[<AttributeUsage(AttributeTargets.Property)>]
type MinimumAttribute (minimum: double, [<Optional; DefaultParameterValue(false)>] exclusive: bool) =
    inherit Attribute()
    member val Minimum = minimum
    member val Exclusive = exclusive

[<AttributeUsage(AttributeTargets.Property)>]
type MaximumAttribute (maximum: double, [<Optional; DefaultParameterValue(false)>] exclusive: bool) =
    inherit Attribute()
    member val Maximum = maximum
    member val Exclusive = exclusive

// String

[<AttributeUsage(AttributeTargets.Property)>]
type MinLengthAttribute (minLength: int) =
    inherit Attribute()
    member val MinLength = minLength

[<AttributeUsage(AttributeTargets.Property)>]
type MaxLengthAttribute (maxLength: int) =
    inherit Attribute()
    member val MaxLength = maxLength

[<AttributeUsage(AttributeTargets.Property)>]
type PatternAttribute (pattern: string) =
    inherit Attribute()
    member val Pattern = pattern

// Array

[<AttributeUsage(AttributeTargets.Property)>]
type MinItemsAttribute (minItems: int) =
    inherit Attribute()
    member val MinItems = minItems

[<AttributeUsage(AttributeTargets.Property)>]
type MaxItemsAttribute (maxItems: int) =
    inherit Attribute()
    member val MaxItems = maxItems

[<AttributeUsage(AttributeTargets.Property)>]
type UniqueItemsAttribute ([<Optional; DefaultParameterValue(true)>] uniqueItems: bool) =
    inherit Attribute()
    member val UniqueItems = uniqueItems

// Hyperschema

[<AttributeUsage(AttributeTargets.Class ||| AttributeTargets.Struct ||| AttributeTargets.Property,
  AllowMultiple = true)>]
type LinkAttribute
    ( rel: string,
      href: string,
      [<Optional; DefaultParameterValue([| |])>] templateRequired: string[],
      [<Optional; DefaultParameterValue(null)>] hrefSchema: Type,
      [<Optional; DefaultParameterValue(null)>] targetHints: obj,
      [<Optional; DefaultParameterValue(null)>] targetMediaType: string) =
    inherit Attribute()
    member val Rel = rel
    member val Href = href
    member val TemplateRequired = templateRequired
    member val HrefSchema = hrefSchema
    member val TargetHints = targetHints
    member val TargetMediaType = targetMediaType


[<AttributeUsage(AttributeTargets.Class ||| AttributeTargets.Struct ||| AttributeTargets.Property,
  AllowMultiple = true)>]
type TemplatePointerAttribute (variableName: string, pointer: string) =
    inherit Attribute()
    member val VariableName = variableName
    member val Pointer = pointer