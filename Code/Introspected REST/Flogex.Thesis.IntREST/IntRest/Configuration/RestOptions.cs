using System.Text.Json;
using Flogex.Thesis.IntRest.Serialization;

namespace Flogex.Thesis.IntRest.Configuration
{
    public class RestOptions
    {
        public RestOptions()
        {
            this.JsonSerializerOptions = new JsonSerializerOptions
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            this.JsonSerializerOptions.Converters.Add(new EnrichedRestResultJsonConverter());

            this.Microtypes = new MicrotypeRegistry();
        }

        public JsonSerializerOptions JsonSerializerOptions { get; set; }

        public JsonWriterOptions JsonWriterOptions => new JsonWriterOptions
        {
            Encoder = this.JsonSerializerOptions.Encoder,
            Indented = this.JsonSerializerOptions.WriteIndented,
            SkipValidation = false
        };

        public MicrotypeRegistry Microtypes { get; }
    }
}
