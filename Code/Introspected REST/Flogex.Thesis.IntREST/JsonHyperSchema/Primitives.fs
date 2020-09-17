namespace Flogex.Thesis.JsonHyperSchema.Primitives

open System

type NonNegativeInt =
    | NonNegativeInt of int

    [<CompiledName("Create")>]
    static member create i =
        if i < 0 then raise (invalidArg "i" "i must be a non-negative integer.")
        else NonNegativeInt i

    static member op_Implicit value = NonNegativeInt.create value


type PositiveInt =
    | PositiveInt of int

    [<CompiledName("Create")>]
    static member create i =
        if i <= 0 then raise (invalidArg "i" "i must be a non-negative integer.")
        else PositiveInt i

    static member op_Implicit = PositiveInt.create

type MultipleAttributesMap = Map<string, Attribute[]>

type AttributesMap = Map<string, Attribute>

module AttributesMap =
    let contains<'a when 'a :> Attribute> (map: MultipleAttributesMap) =
        map.ContainsKey(typeof<'a>.FullName)

    let find<'a when 'a :> Attribute> (map: AttributesMap) =
        map.TryFind (typeof<'a>.FullName)
        |> Option.filter(fun attr -> attr :? 'a)
        |> Option.map (fun attr -> attr :?> 'a)

    let findAll<'a when 'a :> Attribute> (map: MultipleAttributesMap) =
        map.TryFind (typeof<'a>.FullName)
        |> Option.defaultValue Array.empty<Attribute>
        |> Array.filter(fun attr -> attr :? 'a)
        |> Array.map (fun attr -> attr :?> 'a)

    let empty: MultipleAttributesMap = Map.empty<string, Attribute[]>

type ModelProperty =
    { Name: string
      Type: Type
      Attributes: MultipleAttributesMap }

module ModelProperty =
    let ofType clrType =
        { Name = String.Empty
          Type = clrType
          Attributes = AttributesMap.empty }