using System.Text.Json;
using Flogex.Thesis.IntRest.Serialization;

namespace Flogex.Thesis.IntRest.Content
{
    public class JsonHomeJsonConverter : WriteOnlyJsonConverter<JsonHomeData>
    {
        public override void Write(Utf8JsonWriter writer, JsonHomeData value, JsonSerializerOptions options)
        {
            var namingPolicy = options.PropertyNamingPolicy;
            var mustWriteNullValues = !options.IgnoreNullValues;

            writer.WriteStartObject();

            if (value.Title != null || mustWriteNullValues)
            {
                writer.WriteStartObject(namingPolicy.ConvertName("api"));
                writer.WriteString(namingPolicy.ConvertName("title"), value.Title);
                // No 'links' property because links are provided via introspection
                writer.WriteEndObject();
            }

            writer.WriteStartObject(namingPolicy.ConvertName("resources")); // Empty because links are provided via introspection
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
