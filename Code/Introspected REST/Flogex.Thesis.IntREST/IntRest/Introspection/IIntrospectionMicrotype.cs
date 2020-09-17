namespace Flogex.Thesis.IntRest.Introspection
{
    public interface IIntrospectionMicrotype : IMicrotypeDescriptor, IIntrospectionExecutor
    {
        string IMicrotypeDescriptor.Category => "introspection";
    }
}
