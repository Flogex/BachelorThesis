using System;
using System.Collections.Generic;
using System.Linq;
using Flogex.Thesis.JsonHyperSchema.Attributes;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Notes
{
    [Link("creator", "/authors/{creatorId}",
        templateRequired: new[] { "creatorId" },
        targetMediaType: "application/vnd.microtype-container+json")]
    [TemplatePointer("creatorId", "/creator/id")]
    [Link("keywords", "/notes/{id}/keywords",
        templateRequired: new[] { "id" },
        targetMediaType: "application/vnd.microtype-container+json")]
    [Link("delete", "/notes/{id}",
        templateRequired: new[] { "creatorId" },
        targetMediaType: "application/vnd.microtype-container+json")]
    [Link("contributor", "/authors/{id}")]
    public readonly struct NoteDTO
    {
        private readonly DateTime _lastModified;

        public NoteDTO(int id, string title, string content, CreatorOverviewDTO creator, DateTime dateCreated,
            DateTime lastModified, IEnumerable<ContributorOverviewDTO> contributors)
        {
            this.Id = id;
            this.Title = title;
            this.Content = content;
            this.Creator = creator;
            this.DateCreated = dateCreated;
            this.Contributors = contributors;
            _lastModified = lastModified;
        }

        public int Id { get; }

        [NonNullable]
        public string Title { get; }

        [NonNullable]
        public string Content { get; }

        public CreatorOverviewDTO Creator { get; }

        public DateTime DateCreated { get; }

        public IEnumerable<ContributorOverviewDTO> Contributors { get; }

        public static NoteDTO FromNote(Note note)
        {
            return new NoteDTO(
                note.Id,
                note.Title ?? string.Empty,
                note.Content ?? string.Empty,
                CreatorOverviewDTO.FromAuthor(note.Creator),
                note.DateCreated,
                note.LastModified,
                note.Contributors?.Select(ContributorOverviewDTO.FromAuthor) ?? Enumerable.Empty<ContributorOverviewDTO>());
        }

        public readonly struct CreatorOverviewDTO
        {
            public CreatorOverviewDTO(int id, string givenName, string familyName)
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

            public static CreatorOverviewDTO FromAuthor(Author author)
                => new CreatorOverviewDTO(author.Id, author.GivenName, author.FamilyName);
        }

        public readonly struct ContributorOverviewDTO
        {
            public ContributorOverviewDTO(int id, string givenName, string familyName)
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

            public static ContributorOverviewDTO FromAuthor(Author author)
                => new ContributorOverviewDTO(author.Id, author.GivenName, author.FamilyName);
        }
    }
}
