using System.Text.Json;

namespace Flogex.Thesis.IntRest.Runtime
{
    public class PaginationMicrotype : IRuntimeMicrotype
    {
        public PaginationMicrotype(
            string currentUrl,
            int currentPage,
            int pageSize,
            int maxResults)
            : this(new PaginationData(currentUrl, currentPage, pageSize, maxResults)) { }

        public PaginationMicrotype(PaginationData data)
        {
            this.Metadata = data;
        }

        public PaginationData Metadata { get; }

        void IRuntimeMicrotype.Write(Utf8JsonWriter writer, JsonSerializerOptions options)
            => new PaginationJsonConverter().Write(writer, this.Metadata, options);
    }
}
