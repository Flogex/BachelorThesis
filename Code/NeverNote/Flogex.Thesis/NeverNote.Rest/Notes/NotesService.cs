using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Rest.Authors;
using Flogex.Thesis.NeverNote.Shared.Data;
using JsonApiDotNetCore.Exceptions;
using JsonApiDotNetCore.Models.JsonApiDocuments;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace Flogex.Thesis.NeverNote.Rest.Notes
{
    public class NotesService : IResourceService<Note>
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotesService(NotesRepository notes, AuthorsRepository authors, IHttpContextAccessor httpContextAccessor)
        {
            _notes = notes;
            _authors = authors;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Note> CreateAsync(Note entity)
        {
            var context = _httpContextAccessor.HttpContext;
            var userName = context.GetUserName();

            if (userName is null)
                throw new JsonApiException(new Error(HttpStatusCode.Unauthorized));

            var creator = await _authors.GetByUserName(userName);

            if (!creator.HasValue)
                throw new JsonApiException(new Error(HttpStatusCode.Unauthorized));

            var input = new NotesRepository.NoteInput(
                title: entity.Title ?? throw new JsonApiException(new Error(HttpStatusCode.BadRequest)),
                content: entity.Content ?? throw new JsonApiException(new Error(HttpStatusCode.BadRequest)));

            var addedNote = await _notes.AddNote(input, creator.Value);

            return Note.FromNote(addedNote);
        }
        public async Task<IReadOnlyCollection<Note>> GetAsync()
        {
            var notes = await _notes.GetAll();
            return notes.Select(Note.FromNote).ToList().AsReadOnly();
        }

        public async Task<Note> GetAsync(int id)
        {
            var result = await _notes.GetById(id);
            return result.HasValue
                ? Note.FromNote(result.Value)
                : null;
        }

        public Task DeleteAsync(int id)
            => _notes.Remove(id).AsTask();

        public async Task<object> GetSecondaryAsync(int noteId, string relationshipName)
        {
            var note = await _notes.GetById(noteId);

            if (!note.HasValue)
                throw new JsonApiException(new Error(HttpStatusCode.NotFound));

            if (relationshipName == "creator")
                return Author.FromAuthor(note.Value.Creator);

            if (relationshipName == "contributors")
                return note.Value.Contributors.Select(Author.FromAuthor);

            throw new NotImplementedException();
        }

        public Task<Note> UpdateAsync(int id, Note resource) => throw new NotImplementedException();
        public Task UpdateRelationshipAsync(int id, string relationshipName, object relationships) => throw new NotImplementedException();
        Task<Note> IGetRelationshipService<Note, int>.GetRelationshipAsync(int id, string relationshipName) => throw new NotImplementedException();
    }
}
