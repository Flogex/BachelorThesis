namespace Flogex.Thesis.IntRest.Runtime
{
    public static class RestResultExtensions
    {
        public static RestResult AddPagination(
            this RestResult result,
            string url,
            int currentPage,
            int pageSize,
            int maxResults)
            => result.AddRuntimeMicrotype(new PaginationMicrotype(url, currentPage, pageSize, maxResults));
    }
}
