using System;

namespace Flogex.Thesis.IntRest.Conneg
{
    public readonly struct MicrotypeDescriptor : IMicrotypeDescriptor
    {
        public MicrotypeDescriptor(string category, string identifier)
        {
            this.Category = category ?? throw new ArgumentNullException(nameof(category));
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        public string Category { get; }

        public string Identifier { get; }
    }
}
