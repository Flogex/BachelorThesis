using System;

namespace Flogex.Thesis.IntRest.Runtime
{
    public class RuntimeMicrotypeDescriptor : IMicrotypeDescriptor
    {
        public RuntimeMicrotypeDescriptor(string category, string identifier, Type runtimeMicrotype)
        {
            this.Category = category ?? throw new ArgumentNullException(nameof(category));
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            this.RuntimeMicrotype = runtimeMicrotype ?? throw new ArgumentNullException(nameof(runtimeMicrotype));
        }

        public string Category { get; }

        public string Identifier { get; }

        public Type RuntimeMicrotype { get; }

        public override bool Equals(object? obj)
            => (this as IMicrotypeDescriptor).Equals(obj as IMicrotypeDescriptor);

        public override int GetHashCode() => HashCode.Combine(this.Category, this.Identifier);
    }

    public class RuntimeMicrotypeDescriptor<T> : RuntimeMicrotypeDescriptor where T : IRuntimeMicrotype
    {
        public RuntimeMicrotypeDescriptor(string category, string identifier)
            : base(category, identifier, typeof(T)) { }
    }
}
