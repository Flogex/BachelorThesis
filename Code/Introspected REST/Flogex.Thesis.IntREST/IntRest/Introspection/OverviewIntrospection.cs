using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.IntRest.Configuration;
using Flogex.Thesis.IntRest.Content;
using Flogex.Thesis.IntRest.Runtime;
using Flogex.Thesis.IntRest.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class OverviewIntrospection : IIntrospectionMicrotype
    {
        private readonly int _statusCode;
        private readonly RestOptions _options;

        public OverviewIntrospection(int statusCode, RestOptions options)
        {
            _statusCode = statusCode;
            _options = options;
        }

        public string Identifier { get; } = "overview-introspection";

        public async Task ExecuteAsync(ControllerActionDescriptor action, HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = _statusCode;
            response.ContentType = "application/vnd.microtype-container+json;introspection=overview";
            await response.StartAsync();

            await WriteAvailableMicrotypesOverview(action, response.Body);

            await response.CompleteAsync();
        }

        private Task WriteAvailableMicrotypesOverview(ControllerActionDescriptor action, Stream stream)
        {
            var method = action.MethodInfo;

            var contentMicrotypes = GetContentMicrotypesForAction(method);
            var runtimeMicrotypes = GetRuntimeMicrotypesForAction(method);
            var introspectionMicrotypes = _options.Microtypes.IntrospectionMicrotypes;

            var overview = new IntrospectionOverviewData(contentMicrotypes, runtimeMicrotypes, introspectionMicrotypes);

            var enrichedRestResult = new EnrichedRestResult(
                new IntrospectionOverviewMicrotype(overview),
                Enumerable.Empty<IRuntimeMicrotype>());

            return JsonSerializer.SerializeAsync(
                stream,
                enrichedRestResult,
                _options.JsonSerializerOptions);
        }

        private IEnumerable<string> GetContentMicrotypesForAction(MethodInfo action)
        {
            return Attribute.GetCustomAttributes(action, typeof(SupportsContentMicrotypeAttribute), false)
                .Cast<SupportsContentMicrotypeAttribute>()
                .Select(attr => attr.Identifier);
        }

        private IEnumerable<RuntimeMicrotypeDescriptor> GetRuntimeMicrotypesForAction(MethodInfo action)
        {
            return Attribute.GetCustomAttributes(action, typeof(SupportsRuntimeMicrotypeAttribute), false)
                .Cast<SupportsRuntimeMicrotypeAttribute>()
                .Select(attr => attr.Descriptor);
        }
    }

    internal class IntrospectionOverviewData
    {
        public IntrospectionOverviewData(
            IEnumerable<string> content,
            IEnumerable<RuntimeMicrotypeDescriptor> runtime,
            IEnumerable<IIntrospectionMicrotype> introspection)
        {
            this.Content = content ?? Enumerable.Empty<string>();
            this.Runtime = runtime ?? Enumerable.Empty<RuntimeMicrotypeDescriptor>();
            this.Introspection = introspection ?? Enumerable.Empty<IIntrospectionMicrotype>();
        }

        public IEnumerable<string> Content { get; }

        public IEnumerable<RuntimeMicrotypeDescriptor> Runtime { get; }

        public IEnumerable<IIntrospectionMicrotype> Introspection { get; }
    }

    internal class IntrospectionOverviewMicrotype : IContentMicrotype
    {
        public IntrospectionOverviewMicrotype(IntrospectionOverviewData overview)
        {
            this.Content = overview ?? throw new ArgumentNullException(nameof(overview));
        }

        public IntrospectionOverviewData Content { get; }

        public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
            => new IntrospectionOverviewJsonConverter().Write(writer, this.Content, options);
    }

    internal class IntrospectionOverviewJsonConverter : WriteOnlyJsonConverter<IntrospectionOverviewData>
    {
        public override void Write(Utf8JsonWriter writer, IntrospectionOverviewData value, JsonSerializerOptions options)
        {
            var namingPolicy = options.PropertyNamingPolicy;
            var mustWriteNullValues = !options.IgnoreNullValues;

            writer.WriteStartObject();

            if (value.Content.Any() || mustWriteNullValues)
            {
                writer.WriteStartObject(namingPolicy.ConvertName("content"));

                foreach (var microtype in value.Content)
                {
                    writer.WriteStartObject(microtype);
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
            }

            if (value.Runtime.Any() || mustWriteNullValues)
            {
                writer.WriteStartObject(namingPolicy.ConvertName("runtime"));

                foreach (var microtype in value.Runtime)
                {
                    writer.WriteStartObject(microtype.Identifier);
                    writer.WriteString(namingPolicy.ConvertName("category"), microtype.Category);
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
            }

            if (value.Introspection.Any() || mustWriteNullValues)
            {
                writer.WriteStartObject(namingPolicy.ConvertName("introspection"));

                foreach (var microtype in value.Introspection)
                {
                    writer.WriteStartObject(microtype.Identifier);
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}
