using System;
using System.Collections;
using System.Collections.Generic;
using Flogex.Thesis.JsonHyperSchema.Attributes;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Notes
{
    public readonly struct NoteOverviewDTO
    {
        public NoteOverviewDTO(int id, string title)
        {
            this.Id = id;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        public int Id { get; }

        [NonNullable]
        public string Title { get; }

        public static NoteOverviewDTO FromNote(Note note)
        {
            return new NoteOverviewDTO(
                note.Id,
                note.Title ?? string.Empty);
        }
    }

    [Link("add", "/notes",
        targetMediaType: "application/vnd.microtype-container+json")]
    [Link("item", "/notes/{id}",
        templateRequired: new[] { "id" },
        targetMediaType: "application/vnd.microtype-container+json")]
    public readonly struct NotesOverviewDTO : IEnumerable<NoteOverviewDTO>
    {
        private readonly IEnumerable<NoteOverviewDTO> _other;

        public NotesOverviewDTO(IEnumerable<NoteOverviewDTO> other)
        {
            this._other = other;
        }

        public IEnumerator<NoteOverviewDTO> GetEnumerator()
            => _other.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _other.GetEnumerator();
    }
}
