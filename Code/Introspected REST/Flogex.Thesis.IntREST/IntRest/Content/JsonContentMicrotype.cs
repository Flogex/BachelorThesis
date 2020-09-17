using System;
using System.Text.Json;
using Flogex.Thesis.IntRest.Serialization;

namespace Flogex.Thesis.IntRest.Content
{
    public class JsonContentMicrotype : IContentMicrotype
    {
        public JsonContentMicrotype(object content)
        {
            this.Content = content;
        }

        public object Content { get; }

        public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
            => new DefaultJsonConverter<object>().Write(writer, this.Content, options);
    }

    public class JsonContentMicrotypeDescriptor : ContentMicrotypeDescriptor
    {
        public JsonContentMicrotypeDescriptor() : base("json") { }

        public override bool CanHandle(Type type) => true;

        public override IContentMicrotype GetMicrotype(object data)
            => new JsonContentMicrotype(data);
    }
}
