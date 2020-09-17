using System.Text.Json;

namespace Flogex.Thesis.IntRest.Serialization
{
    public class PlainTextConverter : WriteOnlyJsonConverter<string>
    {
        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            => writer.WriteStringValue(value);
    }
}
