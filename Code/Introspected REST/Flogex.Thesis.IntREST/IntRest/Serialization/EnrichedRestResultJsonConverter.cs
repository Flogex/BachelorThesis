using System.Text.Json;

namespace Flogex.Thesis.IntRest.Serialization
{
    internal class EnrichedRestResultJsonConverter : WriteOnlyJsonConverter<EnrichedRestResult>
    {
        public override void Write(
            Utf8JsonWriter writer,
            EnrichedRestResult value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            value.ContentMicrotype.Write(writer, options);

            writer.WriteStartObject("meta");

            foreach (var component in value.RuntimeMicrotypes)
                component.Write(writer, options);

            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
