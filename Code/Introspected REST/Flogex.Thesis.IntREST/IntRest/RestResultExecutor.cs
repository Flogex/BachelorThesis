using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.IntRest.Configuration;
using Flogex.Thesis.IntRest.Conneg;
using Flogex.Thesis.IntRest.Content;
using Flogex.Thesis.IntRest.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Flogex.Thesis.IntRest
{
    internal class RestResultExecutor : IActionResultExecutor<RestResult>
    {
        private readonly RestOptions _options;

        public RestResultExecutor(RestOptions options)
        {
            _options = options;
        }

        public async Task ExecuteAsync(ActionContext context, RestResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var acceptedMicrotypes = context.HttpContext.Items[MicrotypeCollection.Identifier] as MicrotypeCollection
                ?? new MicrotypeCollection();

            var acceptedContentMicrotypes = acceptedMicrotypes
                .Where(m => m.Category == "content")
                .Select(m => m.Identifier)
                .ToList();

            var endpoint = context.HttpContext.GetEndpoint();
            var executedMethod = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.MethodInfo;

            var supportedContentMicrotypes = executedMethod?
                .GetCustomAttributes<SupportsContentMicrotypeAttribute>()
                .Select(attr => attr.Identifier)
                .ToList();

            var possibleContentMicrotypes = _options.Microtypes.ContentMicrotypes
                .Where(m => acceptedContentMicrotypes.Contains(m.Identifier))
                .Where(m => supportedContentMicrotypes?.Contains(m.Identifier) == true)
                .ToList();

            if (possibleContentMicrotypes is null || possibleContentMicrotypes.Count == 0)
            {
                possibleContentMicrotypes = new List<ContentMicrotypeDescriptor>(1)
                {
                    new JsonContentMicrotypeDescriptor()
                };
            }

            ContentMicrotypeDescriptor? selectedContentMicrotype = null;
            var resultDataType = result.Data.GetType();
            foreach (var contentMicrotype in possibleContentMicrotypes)
            {
                if (contentMicrotype.CanHandle(resultDataType))
                    selectedContentMicrotype = contentMicrotype;
            }
            selectedContentMicrotype ??= new JsonContentMicrotypeDescriptor();

            var acceptedRuntimeMicrotypes = _options.Microtypes.RuntimeMicrotypes
                .Where(m => acceptedMicrotypes.Contains(m.Category, m.Identifier))
                .ToList();

            var runtimeMicrotypes = new List<IRuntimeMicrotype>();
            var runtimeMicrotypesDescriptors = new List<RuntimeMicrotypeDescriptor>();

            foreach (var microtype in result.RuntimeMicrotypes)
                foreach (var acceptedMicrotypeDescriptor in acceptedRuntimeMicrotypes)
                    if (acceptedMicrotypeDescriptor.RuntimeMicrotype == microtype.GetType())
                    {
                        runtimeMicrotypes.Add(microtype);
                        runtimeMicrotypesDescriptors.Add(acceptedMicrotypeDescriptor);
                        break;
                    }

            var enrichedRestResult = new EnrichedRestResult(
                selectedContentMicrotype.GetMicrotype(result.Data),
                runtimeMicrotypes);

            var response = context.HttpContext.Response;
            response.StatusCode = result.StatusCode;
            response.ContentType = GetContentTypeHeader(selectedContentMicrotype, runtimeMicrotypesDescriptors);
            await response.StartAsync();

            await JsonSerializer.SerializeAsync(
                response.Body,
                enrichedRestResult,
                _options.JsonSerializerOptions);

            await response.CompleteAsync();
        }

        private string GetContentTypeHeader(ContentMicrotypeDescriptor contentDescriptor, IEnumerable<RuntimeMicrotypeDescriptor> runtimeDescriptors)
        {
            return new IMicrotypeDescriptor[] { contentDescriptor }
                .Concat(runtimeDescriptors)
                .Aggregate(
                    seed: new StringBuilder("application/vnd.microtypes-container+json"),
                    func: (builder, descriptor) => builder
                        .Append(';')
                        .Append(descriptor.Category)
                        .Append('=')
                        .Append(descriptor.Identifier),
                    resultSelector: builder => builder.ToString());
        }
    }
}
