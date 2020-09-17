using Flogex.Thesis.JsonHyperSchema.Attributes;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Authors
{
    [Link("item", "/authors/{id}",
        templateRequired: new[] { "id" },
        targetMediaType: "application/vnd.microtype-container+json")]
    public readonly struct AuthorOverviewDTO
    {
        public AuthorOverviewDTO(int id, string givenName, string familyName)
        {
            this.Id = id;
            this.GivenName = givenName;
            this.FamilyName = familyName;
        }

        public int Id { get; }

        [NonNullable]
        public string GivenName { get; }

        [NonNullable]
        public string FamilyName { get; }

        public static AuthorOverviewDTO FromAuthor(Author author)
            => new AuthorOverviewDTO(author.Id, author.GivenName, author.FamilyName);
    }
}
