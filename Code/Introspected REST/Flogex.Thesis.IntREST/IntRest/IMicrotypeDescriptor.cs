using System;

namespace Flogex.Thesis.IntRest
{
    public interface IMicrotypeDescriptor : IEquatable<IMicrotypeDescriptor>
    {
        string Category { get; }

        string Identifier { get; }

        bool IEquatable<IMicrotypeDescriptor>.Equals(IMicrotypeDescriptor? other)
            => other != null && this.Category == other.Category && this.Identifier == other.Identifier;
    }
}
