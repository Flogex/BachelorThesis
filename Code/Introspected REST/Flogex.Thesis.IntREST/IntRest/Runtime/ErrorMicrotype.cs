using System.Text.Json;

namespace Flogex.Thesis.IntRest.Runtime
{
    public class ErrorMicrotype : IRuntimeMicrotype
    {
        public ErrorMicrotype(int errorCode, string message)
            : this(new ErrorData(errorCode, message)) { }

        public ErrorMicrotype(ErrorData error)
        {
            this.Metadata = error;
        }

        public ErrorData Metadata { get; }

        public void Write(Utf8JsonWriter writer, JsonSerializerOptions options)
            => new ErrorJsonConverter().Write(writer, this.Metadata, options);
    }
}
