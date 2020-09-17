using Flogex.Thesis.JsonHyperSchema.Attributes;

namespace Flogex.Thesis.IntRest.Content
{
    [Link("notes", "/notes")]
    [Link("authors", "/authors")]
    [Link("searchNotes", "/notes/search")]
    public class JsonHomeData
    {
        public JsonHomeData(string? title)
        {
            this.Title = title;
        }

        public string? Title { get; set; }
    }
}
