using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.GraphQL.Serialization
{
    public class KeywordJsonConverter : JsonConverter<Keyword>
    {
        public override bool CanConvert(Type typeToConvert)
            => base.CanConvert(typeToConvert);

        public override Keyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert != typeof(Keyword))
                throw new ArgumentException("Can only convert to Keyword.", nameof(typeToConvert));

            var value = reader.GetString();
            return new Keyword(value);
        }

        public override void Write(Utf8JsonWriter writer, Keyword value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.Value);
    }
}
