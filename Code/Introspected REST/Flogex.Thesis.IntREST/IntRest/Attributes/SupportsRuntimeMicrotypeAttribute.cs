using System;
using Flogex.Thesis.IntRest.Runtime;

namespace Flogex.Thesis.IntRest.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SupportsRuntimeMicrotypeAttribute : Attribute
    {
        public SupportsRuntimeMicrotypeAttribute(string category, string identifier, Type runtimeMicrotype)
            : this(new RuntimeMicrotypeDescriptor(category, identifier, runtimeMicrotype)) { }

        public SupportsRuntimeMicrotypeAttribute(RuntimeMicrotypeDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }

        public RuntimeMicrotypeDescriptor Descriptor { get; }
    }
}
