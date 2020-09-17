using System.Text.Json;
using Flogex.Thesis.IntRest.Serialization;

namespace Flogex.Thesis.IntRest.Runtime
{
    public class ErrorJsonConverter : WriteOnlyJsonConverter<ErrorData>
    {
        public override void Write(Utf8JsonWriter writer, ErrorData value, JsonSerializerOptions options)
        {
            var namingPolicy = options.PropertyNamingPolicy;
            var mustWriteNullValues = !options.IgnoreNullValues;

            writer.WriteStartObject(namingPolicy.ConvertName("simple-error"));

            writer.WriteNumber(namingPolicy.ConvertName("code"), value.ErrorCode);

            if (value.Message != null || mustWriteNullValues)
                writer.WriteString(namingPolicy.ConvertName("message"), value.Message);

            writer.WriteEndObject();
        }
    }
}
