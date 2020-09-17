using Flogex.Thesis.JsonHyperSchema.Attributes;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Notes
{
    [Link("byTitle", "/notes/search/byTitle?title={title}",
        templateRequired: new[] { "title" },
        targetMediaType: "application/vnd.microtype-container+json")]
    [Link("byKeyword", "/notes/search/byKeyword?keyword={keyword}",
        templateRequired: new[] { "keyword" },
        targetMediaType: "application/vnd.microtype-container+json")]
    [Link("byAuthor", "/notes/search/byAuthor?authorId={authorId}",
        templateRequired: new[] { "authorId" },
        targetMediaType: "application/vnd.microtype-container+json")]
    public readonly struct SearchOverviewDTO
    {
    }
}
