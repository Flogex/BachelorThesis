using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flogex.Thesis.IntRest.Serialization
{
    public abstract class WriteOnlyJsonConverter<T> : JsonConverter<T>
    {
        public override T Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
            => throw new NotImplementedException("This JsonConverter is write-only.");
    }
}
