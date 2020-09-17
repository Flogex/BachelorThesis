namespace Flogex.Thesis.IntRest.Runtime
{
    public class PaginationData
    {
        public PaginationData(
            string currentUrl,
            int currentPage,
            int pageSize,
            int maxResults)
        {
            this.CurrentUrl = currentUrl;
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;

            this.PreviousPage = currentPage > 1
                ? currentPage - 1
                : (int?)null;

            this.NextPage = currentPage * pageSize < maxResults
                ? currentPage + 1
                : (int?)null;
        }

        public string CurrentUrl { get; }

        public int PageSize { get; }

        public int CurrentPage { get; }

        public int? PreviousPage { get; }

        public int? NextPage { get; }
    }
}
