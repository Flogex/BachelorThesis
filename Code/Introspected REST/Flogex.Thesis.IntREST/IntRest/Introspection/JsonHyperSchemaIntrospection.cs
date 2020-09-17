using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.IntRest.Configuration;
using Flogex.Thesis.IntRest.Conneg;
using Flogex.Thesis.IntRest.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class JsonHyperSchemaIntrospection : IIntrospectionMicrotype
    {
        private readonly RestOptions _options;

        public JsonHyperSchemaIntrospection(RestOptions options)
        {
            _options = options;
        }

        public string Identifier { get; } = "json-hyper-schema";

        public async Task ExecuteAsync(ControllerActionDescriptor action, HttpContext context)
        {
            var (executor, microtype) = SelectExecutor(action, context);

            var response = context.Response;
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = $"application/vnd.microtype-container+json;introspection={this.Identifier};" +
                $"content={microtype.Identifier}";

            await response.StartAsync();

            var writer = new Utf8JsonWriter(response.Body, _options.JsonWriterOptions);

            writer.WriteStartObject();
            writer.WritePropertyName("data");

            await executor.ExecuteAsync(action, writer, _options.JsonSerializerOptions);

            writer.WriteStartObject("meta");
            writer.WriteEndObject();
            writer.WriteEndObject();

            await writer.FlushAsync();

            await response.CompleteAsync();
        }

        private (IContentJsonSchemaIntrospectionExecutor executor, ContentMicrotypeDescriptor microtype)
            SelectExecutor(ControllerActionDescriptor action, HttpContext context)
        {
            var method = action.MethodInfo;
            var supportedContentMicrotypes = method.GetCustomAttributes<SupportsContentMicrotypeAttribute>()
                .Select(_ => _.Identifier)
                .ToList();

            var acceptedMicrotypes = context.Items[MicrotypeCollection.Identifier] as MicrotypeCollection;
            var acceptedContentMicrotypes = acceptedMicrotypes
                .Where(_ => _.Category == "content")
                .Select(_ => _.Identifier)
                .ToList();

            var possibleContentMicrotypes = _options.Microtypes.ContentMicrotypes
                .Where(m => (!acceptedContentMicrotypes.Any() || acceptedContentMicrotypes.Contains(m.Identifier)) &&
                    supportedContentMicrotypes.Contains(m.Identifier));

            var executors = _options.Microtypes.ContentIntrospectionExecutors
                .OfType<IContentJsonSchemaIntrospectionExecutor>()
                .OrderBy(_ => _.Priority)
                .ToList();

            foreach (var microtype in possibleContentMicrotypes)
                foreach (var executor in executors)
                    if (executor.AcceptsContentMicrotype(microtype))
                        return (executor, microtype);

            return (new PlainJsonJsonSchemaIntrospection(), new JsonContentMicrotypeDescriptor());
        }
    }
}
