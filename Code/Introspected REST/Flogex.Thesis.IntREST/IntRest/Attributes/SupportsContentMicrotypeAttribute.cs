using System;

namespace Flogex.Thesis.IntRest.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SupportsContentMicrotypeAttribute : Attribute
    {
        public SupportsContentMicrotypeAttribute(string identifier)
        {
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        public string Identifier { get; }
    }
}
