using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.IntRest.Configuration;
using Flogex.Thesis.JsonHyperSchema;
using Flogex.Thesis.JsonHyperSchema.JsonTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.FSharp.Core;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class LinksOnlyIntrospection : IIntrospectionMicrotype
    {
        private readonly RestOptions _options;

        public LinksOnlyIntrospection(RestOptions options)
        {
            _options = options;
        }

        public string Identifier { get; } = "links-only";

        public async Task ExecuteAsync(ControllerActionDescriptor action, HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/vnd.microtype-container+json;introspection=links-introspection";

            await response.StartAsync();

            var writer = new Utf8JsonWriter(response.Body, _options.JsonWriterOptions);
            WriteIntrospectionResponse(action, writer, _options.JsonSerializerOptions);

            await writer.FlushAsync();

            await response.CompleteAsync();
        }

        private void WriteIntrospectionResponse(ControllerActionDescriptor action, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            var returnType = action.MethodInfo.GetCustomAttribute<ReturnsAttribute>()?.ReturnType ?? typeof(object);
            var links = Links.ForType(FSharpFunc<Type, InputJsonSchema>.FromConverter(SchemaGenerator.getInputSchema), returnType);

            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteStartObject();
            SchemaSerializer.writeLinks(links).Invoke(Tuple.Create(writer, options));
            writer.WriteEndObject();

            writer.WriteStartObject("meta");
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
