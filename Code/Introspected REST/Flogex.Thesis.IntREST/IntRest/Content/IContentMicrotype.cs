using System.Text.Json;

namespace Flogex.Thesis.IntRest.Content
{
    public interface IContentMicrotype
    {
        void Write(Utf8JsonWriter writer, JsonSerializerOptions options);
    }
}