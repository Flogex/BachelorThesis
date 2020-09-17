using System.Text.Json;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Content;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Flogex.Thesis.IntRest.Introspection
{
    public interface IContentIntrospectionExecutor
    {
        int Priority { get; }

        bool AcceptsContentMicrotype(ContentMicrotypeDescriptor microtype);

        bool AcceptsIntrospectionMicrotype(IIntrospectionMicrotype microtype);

        Task ExecuteAsync(ControllerActionDescriptor action, Utf8JsonWriter writer, JsonSerializerOptions options);
    }
}
