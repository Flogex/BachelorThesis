using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.IntRest.Content;
using Flogex.Thesis.JsonHyperSchema;
using Flogex.Thesis.JsonHyperSchema.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class PlainJsonJsonSchemaIntrospection : IContentJsonSchemaIntrospectionExecutor
    {
        public int Priority { get; } = 10;

        public bool AcceptsContentMicrotype(ContentMicrotypeDescriptor microtype)
            => true;

        public Task ExecuteAsync(
            ControllerActionDescriptor action,
            Utf8JsonWriter writer,
            JsonSerializerOptions options)
        {
            var method = action.MethodInfo;
            var returnType = method.GetCustomAttribute<ReturnsAttribute>()?.ReturnType;

            if (returnType != null)
            {
                var wrapperType = typeof(MicrotypeContainerWrapper<>).MakeGenericType(returnType);
                var schema = SchemaGenerator.GenerateFromType(wrapperType);
                SchemaSerializer.Serialize(schema, writer, options);
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }

            return Task.CompletedTask;
        }

        private class MicrotypeContainerWrapper<T>
        {
            public T Data { get; set; }

            [NonNullable]
            public object Meta { get; set; }
        }
    }
}
