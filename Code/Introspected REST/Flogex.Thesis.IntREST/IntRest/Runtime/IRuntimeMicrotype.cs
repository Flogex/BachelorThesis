using System.Text.Json;

namespace Flogex.Thesis.IntRest.Runtime
{
    public interface IRuntimeMicrotype
    {
        void Write(Utf8JsonWriter writer, JsonSerializerOptions options);
    }
}
