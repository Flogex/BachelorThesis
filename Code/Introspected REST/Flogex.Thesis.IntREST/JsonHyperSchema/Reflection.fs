module internal Flogex.Thesis.JsonHyperSchema.Reflection

open System
open System.Collections
open System.Collections.Generic
open System.Reflection

let private isOneOf values item = Array.contains item values

let private isInteger = isOneOf [| typeof<byte>; typeof<sbyte>; typeof<int16>; typeof<uint16>;
    typeof<int>; typeof<uint32>; typeof<int64>; typeof<uint64> |]

let private isFloatingPointNumber = isOneOf [| typeof<single>; typeof<double>; typeof<decimal> |]

let private isEnumerable t = typeof<IEnumerable>.IsAssignableFrom(t)

let (|Bool|Int|Float|String|Enumerable|Date|Object|) clrType =
    if (clrType = typeof<bool>) then
        Bool
    elif (isInteger clrType) then
        Int
    elif (isFloatingPointNumber clrType) then
        Float
    elif (clrType = typeof<string>) then
        String
    elif (isEnumerable clrType) then
        Enumerable
    elif (clrType = typeof<DateTime>) then
        Date
    else Object

let isNullableType (clrType: Type) =
    clrType.IsGenericType && clrType.GetGenericTypeDefinition() = typedefof<System.Nullable<_>>

let (|NullableType|_|) (clrType: Type) =
    if (clrType.IsValueType && isNullableType clrType) then
        let underlyingType = Nullable.GetUnderlyingType(clrType)
        Some underlyingType
    else None

//TODO Interface
let (|Enum|ValueType|Class|) (clrType: Type) =
    if (clrType.IsEnum) then Enum
    elif (clrType.IsValueType) then ValueType
    else Class

let getEnumerableType (enumerable: Type) =
    if not <| isEnumerable enumerable then
        None
    elif (enumerable.IsArray) then
        Some <| enumerable.GetElementType()
    elif (enumerable.IsGenericType && enumerable.GetGenericTypeDefinition() = typedefof<IEnumerable<_>>) then
        Some (enumerable.GetGenericArguments() |> Array.head)
    else
        let chooser (t: Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<IEnumerable<_>>
            then Some (t.GetGenericArguments() |> Array.head)
            else None

        enumerable.GetInterfaces()
        |> Array.tryPick chooser
        |> Option.orElse (Some typeof<obj>)

let getPublicProperties (clrType: Type) =
    let bindingFlags = BindingFlags.Public ||| BindingFlags.Instance
    clrType.GetProperties bindingFlags

let getAllAttributes (member': MemberInfo) =
    member'.GetCustomAttributes(false)
    |> Array.map (fun attr -> attr :?> Attribute)

let getAttributes<'a when 'a :> Attribute> (member': MemberInfo) =
    member'.GetCustomAttributes(typeof<'a>, false)
    |> Array.filter(fun attr -> attr :? 'a)
    |> Array.map (fun attr -> attr :?> 'a)

let getAttribute<'a when 'a :> Attribute and 'a : null> (member': MemberInfo) =
    let attr = member'.GetCustomAttribute(typeof<'a>, false)
    Option.ofObj (attr :?> 'a)
