using System.Text.Json;
using Flogex.Thesis.IntRest.Serialization;

namespace Flogex.Thesis.IntRest.Runtime
{
    public class PaginationJsonConverter : WriteOnlyJsonConverter<PaginationData>
    {
        public override void Write(Utf8JsonWriter writer, PaginationData value, JsonSerializerOptions options)
        {
            var namingPolicy = options.PropertyNamingPolicy;
            var mustWriteNullValues = !options.IgnoreNullValues;

            writer.WriteStartObject(namingPolicy.ConvertName("simple-pagination"));

            if (value.PreviousPage != null || mustWriteNullValues)
                writer.WriteString(namingPolicy.ConvertName("prev"), value.CurrentUrl + "?page=" + value.PreviousPage);

            if (value.NextPage != null || mustWriteNullValues)
                writer.WriteString(namingPolicy.ConvertName("next"), value.CurrentUrl + "?page=" + value.NextPage);

            writer.WriteEndObject();
        }
    }
}
