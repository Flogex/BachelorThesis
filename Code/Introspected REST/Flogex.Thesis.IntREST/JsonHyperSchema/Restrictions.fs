module internal Flogex.Thesis.JsonHyperSchema.JsonTypes.Restrictions

open System.Text.RegularExpressions
open Microsoft.FSharp.Core.Option
open Flogex.Thesis.JsonHyperSchema.Attributes
open Flogex.Thesis.JsonHyperSchema.Primitives
open Flogex.Thesis.JsonHyperSchema.JsonTypes

exception InvalidRestrictionsException of string

[<RequireQualifiedAccess>]
module NumberRestrictions =
    let private getMultipleOfValue =
        AttributesMap.find<MultipleOfAttribute>
        >> map (fun attr -> attr.MultipleOf)
        >> filter (fun value -> value > 0)
        >> map PositiveInt

    let private getMinimumValue =
        AttributesMap.find<MinimumAttribute>
        >> map (fun attr ->
            let min = attr.Minimum
            if attr.Exclusive then ExclusiveLimit min else InclusiveLimit min)

    let private getMaximumValue =
        AttributesMap.find<MaximumAttribute>
        >> map (fun attr ->
             let max = attr.Maximum
             if attr.Exclusive then ExclusiveLimit max else InclusiveLimit max)

    let fromAttributes (original: NumberRestrictions) (attributes: AttributesMap): NumberRestrictions =
        let multipleOf = getMultipleOfValue attributes
        let min = getMinimumValue attributes
        let max = getMaximumValue attributes

        match (min, max) with
        | (Some min', Some max') ->
            match (min', max') with
            | (InclusiveLimit a, InclusiveLimit b) when a > b ->
                raise (InvalidRestrictionsException "Minimum must be less or equal to Maximum.")
            | (InclusiveLimit a, ExclusiveLimit b)
            | (ExclusiveLimit a, InclusiveLimit b)
            | (ExclusiveLimit a, ExclusiveLimit b) when a >= b ->
                raise (InvalidRestrictionsException "Minimum must be less than Maximum.")
            | _ -> ()
        | _ -> ()

        { original with MultipleOf = multipleOf; Minimum = min; Maximum = max }

    let None: NumberRestrictions =
        { MultipleOf = None
          Minimum = None
          Maximum = None }

[<RequireQualifiedAccess>]
module StringRestrictions =
    let private getMinLengthValue =
        AttributesMap.find<MinLengthAttribute>
        >> map (fun attr -> attr.MinLength)
        >> filter (fun value -> value >= 0)
        >> map NonNegativeInt

    let private getMaxLengthValue =
        AttributesMap.find<MaxLengthAttribute>
        >> map (fun attr -> attr.MaxLength)
        >> filter (fun value -> value >= 0)
        >> map NonNegativeInt

    let private getPattern =
        AttributesMap.find<PatternAttribute>
        >> map (fun attr -> new Regex(attr.Pattern))

    let fromAttributes (original: StringRestrictions) (attributes: AttributesMap): StringRestrictions =
        let minLength = getMinLengthValue attributes
        let maxLength = getMaxLengthValue attributes
        let pattern = getPattern attributes

        match (minLength, maxLength) with
        | (Some (NonNegativeInt a), Some (NonNegativeInt b)) when a > b ->
            raise (InvalidRestrictionsException "MinLength must be less or equal to MaxLength.")
        | _ -> ()

        { original with MinLength = minLength; MaxLength = maxLength; Pattern = pattern }

    let None: StringRestrictions =
        { MinLength = None
          MaxLength = None
          Pattern = None
          Format = None
          Enum = Array.empty<string> }

[<RequireQualifiedAccess>]
module ArrayRestrictions =
    let private getMinItems =
        AttributesMap.find<MinItemsAttribute>
        >> map (fun attr -> attr.MinItems)
        >> filter (fun value -> value >= 0)
        >> map NonNegativeInt

    let private getMaxItems =
        AttributesMap.find<MaxItemsAttribute>
        >> map (fun attr -> attr.MaxItems)
        >> filter (fun value -> value >= 0)
        >> map NonNegativeInt

    let private getUnique =
        AttributesMap.find<UniqueItemsAttribute>
        >> map (fun attr -> attr.UniqueItems)

    let fromAttributes (original: ArrayRestrictions) (attributes: AttributesMap): ArrayRestrictions =
        let minItems = getMinItems attributes
        let maxItems = getMaxItems attributes
        let unique = getUnique attributes

        match (minItems, maxItems) with
        | (Some (NonNegativeInt a), Some (NonNegativeInt b)) when a > b ->
            raise (InvalidRestrictionsException "MinItems must be less or equal to MaxItems.")
        | _ -> ()

        { original with MinItems = minItems; MaxItems = maxItems; UniqueItems = unique }

    let None: ArrayRestrictions =
        { MinItems = None
          MaxItems = None
          UniqueItems = None }