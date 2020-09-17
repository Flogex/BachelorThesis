module Flogex.Thesis.JsonHyperSchema.JsonWriter

open System.Text.Json
open Flogex.Thesis.JsonHyperSchema.Primitives

type JsonWriter = Utf8JsonWriter * JsonSerializerOptions

let writeStartObject ((writer, options): JsonWriter) =
    writer.WriteStartObject()
    (writer, options)

let writeEndObject ((writer, options): JsonWriter) =
    writer.WriteEndObject()
    (writer, options)

let writeStartArray ((writer, options): JsonWriter) =
    writer.WriteStartArray()
    (writer, options)

let writeEndArray ((writer, options): JsonWriter) =
    writer.WriteEndArray()
    (writer, options)

let writeBoolean (propertyName: string) (value: bool) ((writer, options): JsonWriter) =
    let propertyName' = options.PropertyNamingPolicy.ConvertName(propertyName)
    writer.WriteBoolean(propertyName', value)
    (writer, options)

let writeNull (propertyName: string) ((writer, options): JsonWriter) =
    writer.WriteNull(propertyName)
    (writer, options)

let writePositiveInt (propertyName: string) (positive: PositiveInt) ((writer, options): JsonWriter) =
    let propertyName' = options.PropertyNamingPolicy.ConvertName(propertyName)
    let (PositiveInt value) = positive
    writer.WriteNumber(propertyName', value)
    (writer, options)

let writeNonNegativeInt (propertyName: string) (nonNegative: NonNegativeInt) ((writer, options): JsonWriter) =
    let propertyName' = options.PropertyNamingPolicy.ConvertName(propertyName)
    let (NonNegativeInt value) = nonNegative
    writer.WriteNumber(propertyName', value)
    (writer, options)

let writeDouble (propertyName: string) (value: double) ((writer, options): JsonWriter) =
    let propertyName' = options.PropertyNamingPolicy.ConvertName(propertyName)
    writer.WriteNumber(propertyName', value)
    (writer, options)

let writeString (propertyName: string) (value: string) ((writer, options): JsonWriter) =
    let propertyName' = options.PropertyNamingPolicy.ConvertName(propertyName)

    if (not (isNull value && options.IgnoreNullValues)) then
        writer.WriteString(propertyName', value)
    else
        ()

    (writer, options)

let writeStringValue (value: string) ((writer, options): JsonWriter) =
    writer.WriteStringValue(value)
    (writer, options)

let writePropertyName (propertyName: string) ((writer, options): JsonWriter) =
    let propertyName' = options.PropertyNamingPolicy.ConvertName(propertyName)
    writer.WritePropertyName(propertyName')
    (writer, options)