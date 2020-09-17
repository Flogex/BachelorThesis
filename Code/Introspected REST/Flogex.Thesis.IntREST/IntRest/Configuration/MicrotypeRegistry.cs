using System.Collections.Generic;
using Flogex.Thesis.IntRest.Content;
using Flogex.Thesis.IntRest.Introspection;
using Flogex.Thesis.IntRest.Runtime;

namespace Flogex.Thesis.IntRest.Configuration
{
    public class MicrotypeRegistry
    {
        private readonly List<IIntrospectionMicrotype> _introspection = new List<IIntrospectionMicrotype>();
        private readonly List<IContentIntrospectionExecutor> _contentIntrospection = new List<IContentIntrospectionExecutor>();
        private readonly List<ContentMicrotypeDescriptor> _content = new List<ContentMicrotypeDescriptor>();
        private readonly List<RuntimeMicrotypeDescriptor> _runtime = new List<RuntimeMicrotypeDescriptor>();

        public IEnumerable<IIntrospectionMicrotype> IntrospectionMicrotypes => _introspection;

        public IEnumerable<IContentIntrospectionExecutor> ContentIntrospectionExecutors => _contentIntrospection;

        public IEnumerable<ContentMicrotypeDescriptor> ContentMicrotypes => _content;

        public IEnumerable<RuntimeMicrotypeDescriptor> RuntimeMicrotypes => _runtime;

        public MicrotypeRegistry RegisterIntrospectionMicrotype(IIntrospectionMicrotype microtype)
        {
            _introspection.Add(microtype);
            return this;
        }

        public MicrotypeRegistry RegisterContentIntrospectionExecutor(IContentIntrospectionExecutor executor)
        {
            _contentIntrospection.Add(executor);
            return this;
        }

        public MicrotypeRegistry RegisterContentMicrotype(ContentMicrotypeDescriptor descriptor)
        {
            _content.Add(descriptor);
            return this;
        }

        public MicrotypeRegistry RegisterRuntimeMicrotype<T>(string category, string identifier)
            where T : IRuntimeMicrotype
        {
            var descriptor = new RuntimeMicrotypeDescriptor<T>(category, identifier);
            return RegisterRuntimeMicrotype(descriptor);
        }

        public MicrotypeRegistry RegisterRuntimeMicrotype(RuntimeMicrotypeDescriptor descriptor)
        {
            _runtime.Add(descriptor);
            return this;
        }
    }
}
