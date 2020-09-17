using System;
using System.Text.Json;

namespace Flogex.Thesis.IntRest.Content
{
    public class JsonHomeMicrotype : IContentMicrotype
    {
        public JsonHomeMicrotype(string? title) : this(new JsonHomeData(title)) { }

        public JsonHomeMicrotype(JsonHomeData content)
        {
            this.Content = content;
        }

        public JsonHomeData Content { get; }

        public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
            => new JsonHomeJsonConverter().Write(writer, this.Content, options);
    }

    public class JsonHomeMicrotypeDescriptor : ContentMicrotypeDescriptor<JsonHomeData>
    {
        public JsonHomeMicrotypeDescriptor() : base("json-home") { }

        public override IContentMicrotype GetMicrotype(object data)
        {
            if (data is JsonHomeData jsonHomeData)
                return new JsonHomeMicrotype(jsonHomeData);

            throw new ArgumentException($"Expected object of type {nameof(jsonHomeData)}, " +
                $"but got {data.GetType().FullName}.", nameof(data));
        }
    }
}
