[<RequireQualifiedAccess>]
module Flogex.Thesis.JsonHyperSchema.Links

open System
open Flogex.Thesis.JsonHyperSchema.Attributes
open Flogex.Thesis.JsonHyperSchema.JsonTypes
open Flogex.Thesis.JsonHyperSchema.Primitives
open Flogex.Thesis.JsonHyperSchema.Reflection
open Tavis.UriTemplates
open System.Reflection

let private toLinkDescriptor getSchema (attr: LinkAttribute) =
    let toArray (obj: obj) =
        match obj with
        | :? array<obj> as arr -> Array.map (fun x -> x.ToString()) arr
        | _ -> Array.singleton (obj.ToString())

    let getTargetHints (hints: obj): Map<string, string[]> =
        getPublicProperties(hints.GetType())
        |> Array.map(fun (prop: PropertyInfo) -> (prop.Name, prop.GetValue hints |> toArray))
        |> Map.ofArray

    { Rel = attr.Rel
      Href = UriTemplate (attr.Href, false, true)
      TemplateRequired =
          if isNull attr.TemplateRequired
          then Array.empty<string>
          else attr.TemplateRequired
      TemplatePointers = Map.empty<string, string>
      HrefSchema = Option.ofObj attr.HrefSchema |> Option.map getSchema
      TargetHints =
        if isNull attr.TargetHints
        then Map.empty<string, string[]>
        else getTargetHints attr.TargetHints
      TargetMediaType = Option.ofObj attr.TargetMediaType }

let private filterPointers (href: UriTemplate) (pointers: Map<string, string>) =
    let templateParams = href.GetParameterNames() |> Set.ofSeq
    let isVariableInTemplate name _ = Set.contains name templateParams
    Map.filter isVariableInTemplate pointers

let private templatePointersForType (t: Type) href =
    let pointers =
        getAttributes<TemplatePointerAttribute> t
        |> Array.map (fun a -> (a.VariableName, a.Pointer))
        |> Map.ofArray

    filterPointers href pointers

let private templatePointersForProperty (prop: ModelProperty) href =
    let pointers =
        AttributesMap.findAll<TemplatePointerAttribute> prop.Attributes
        |> Array.map (fun a -> (a.VariableName, a.Pointer))
        |> Map.ofArray

    filterPointers href pointers

[<CompiledName("ForType")>]
let forType (getSchema: Type -> InputJsonSchema) (t: Type) =

    getAttributes<LinkAttribute> t
    |> Array.map (toLinkDescriptor getSchema)
    |> Array.map (fun link -> { link with TemplatePointers = templatePointersForType t link.Href })

[<CompiledName("ForProperty")>]
let forProperty (getSchema: Type -> InputJsonSchema) (prop: ModelProperty) =
    let propLinks =
        AttributesMap.findAll<LinkAttribute> prop.Attributes
        |> Array.map (toLinkDescriptor getSchema)
        |> Array.map (fun link -> { link with TemplatePointers = templatePointersForProperty prop link.Href })

    let typeLinks = forType getSchema prop.Type

    Array.append propLinks typeLinks
