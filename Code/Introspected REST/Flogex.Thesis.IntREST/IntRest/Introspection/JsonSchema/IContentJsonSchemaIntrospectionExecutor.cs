using System.Text.Json;

namespace Flogex.Thesis.IntRest.Introspection
{
    public interface IContentJsonSchemaIntrospectionExecutor : IContentIntrospectionExecutor
    {
        bool IContentIntrospectionExecutor.AcceptsIntrospectionMicrotype(IIntrospectionMicrotype microtype)
            => microtype.GetType() == typeof(JsonHyperSchemaIntrospection);
    }
}
