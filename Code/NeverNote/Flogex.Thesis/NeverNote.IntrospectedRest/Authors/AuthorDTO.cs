using System;
using System.Collections.Generic;
using System.Linq;
using Flogex.Thesis.JsonHyperSchema.Attributes;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Authors
{
    [Link("item", "/notes/{id}",
        templateRequired: new[] { "id" },
        targetMediaType: "application/vnd.microtype-container+json")]
    public readonly struct AuthorDTO
    {
        public AuthorDTO(int id, string userName, string givenName, string familyName, string email,
            DateTime? birthDate, IEnumerable<EditedNoteOverviewDTO> editedNotes)
        {
            this.Id = id;
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.GivenName = givenName ?? throw new ArgumentNullException(nameof(givenName));
            this.FamilyName = familyName ?? throw new ArgumentNullException(nameof(familyName));
            this.Email = email ?? throw new ArgumentNullException(nameof(email));
            this.BirthDate = birthDate;
            this.EditedNotes = editedNotes ?? throw new ArgumentNullException(nameof(editedNotes));
        }

        public int Id { get; }

        [NonNullable]
        public string UserName { get; }

        [NonNullable]
        public string GivenName { get; }

        [NonNullable]
        public string FamilyName { get; }

        [NonNullable]
        public string Email { get; }

        public DateTime? BirthDate { get; }

        [NonNullable]
        public IEnumerable<EditedNoteOverviewDTO> EditedNotes { get; }

        public static AuthorDTO FromAuthor(Author author, IEnumerable<Note> editedNotes)
        {
            return new AuthorDTO(
                author.Id,
                author.UserName,
                author.GivenName,
                author.FamilyName,
                author.Email,
                author.BirthDate,
                editedNotes?.Select(EditedNoteOverviewDTO.FromNote) ?? Enumerable.Empty<EditedNoteOverviewDTO>());
        }

        public readonly struct EditedNoteOverviewDTO
        {
            public EditedNoteOverviewDTO(int id, string title)
            {
                this.Id = id;
                this.Title = title ?? throw new ArgumentNullException(nameof(title));
            }

            public int Id { get; }

            [NonNullable]
            public string Title { get; }

            public static EditedNoteOverviewDTO FromNote(Note note)
            {
                return new EditedNoteOverviewDTO(
                    note.Id,
                    note.Title ?? string.Empty);
            }
        }
    }
}
