using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.IntRest.Content;
using Flogex.Thesis.JsonHyperSchema;
using Flogex.Thesis.JsonHyperSchema.JsonTypes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.FSharp.Core;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class JsonHomeJsonSchemaIntrospection : IContentJsonSchemaIntrospectionExecutor
    {
        public int Priority { get; } = 0;

        public bool AcceptsContentMicrotype(ContentMicrotypeDescriptor microtype)
            => microtype.Identifier == "json-home";

        public Task ExecuteAsync(
            ControllerActionDescriptor action,
            Utf8JsonWriter writer,
            JsonSerializerOptions options)
        {
            var namingPolicy = options.PropertyNamingPolicy;

            writer.WriteStartObject();
            writer.WriteString(namingPolicy.ConvertName("type"), "object");
            writer.WriteStartObject(namingPolicy.ConvertName("properties"));

            writer.WriteStartObject(namingPolicy.ConvertName("api"));
            writer.WriteString(namingPolicy.ConvertName("type"), "object");
            writer.WriteStartObject(namingPolicy.ConvertName("properties"));
            writer.WriteStartObject(namingPolicy.ConvertName("title"));
            writer.WriteString(namingPolicy.ConvertName("type"), "string");
            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.WriteEndObject();

            writer.WriteStartObject(namingPolicy.ConvertName("resources"));
            writer.WriteString(namingPolicy.ConvertName("type"), "object");
            writer.WriteBoolean(namingPolicy.ConvertName("additionalProperties"), false);
            writer.WriteEndObject();
            writer.WriteEndObject();

            var returnType = action.MethodInfo.GetCustomAttribute<ReturnsAttribute>()?.ReturnType ?? typeof(object);
            var links = Links.ForType(FSharpFunc<Type, InputJsonSchema>.FromConverter(SchemaGenerator.getInputSchema), returnType);
            SchemaSerializer.writeLinks(links).Invoke(Tuple.Create(writer, options));

            writer.WriteEndObject();

            return Task.CompletedTask;
        }
    }
}
