using System;

namespace Flogex.Thesis.IntRest.Content
{
    public abstract class ContentMicrotypeDescriptor : IMicrotypeDescriptor
    {
        public ContentMicrotypeDescriptor(string identifier)
        {
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        public string Category { get; } = "content";

        public string Identifier { get; }

        public abstract bool CanHandle(Type type);

        public abstract IContentMicrotype GetMicrotype(object data);

        public override bool Equals(object? obj)
            => (this as IMicrotypeDescriptor).Equals(obj as IMicrotypeDescriptor);

        public override int GetHashCode() => HashCode.Combine(this.Category, this.Identifier);
    }

    public abstract class ContentMicrotypeDescriptor<T> : ContentMicrotypeDescriptor
    {
        public ContentMicrotypeDescriptor(string identifier) : base(identifier) { }

        public override bool CanHandle(Type type)
            => typeof(T).IsAssignableFrom(type);
    }
}