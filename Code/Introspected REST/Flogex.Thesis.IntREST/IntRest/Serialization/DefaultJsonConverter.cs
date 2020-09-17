using System.Text.Json;

namespace Flogex.Thesis.IntRest.Serialization
{
    public class DefaultJsonConverter<T> : WriteOnlyJsonConverter<T>
    {
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value, options);
    }
}
