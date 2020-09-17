using System;
using System.Text.Json;

namespace Flogex.Thesis.NeverNote.GraphQL.Serialization
{
    public static class JsonSerializationOptions
    {
        public static Action<JsonSerializerOptions> Value =>
            options =>
            {
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.Converters.Insert(0, new KeywordJsonConverter());
                options.WriteIndented = false;
            };
    }
}
