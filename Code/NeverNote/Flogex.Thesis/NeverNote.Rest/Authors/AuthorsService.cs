using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Rest.Notes;
using Flogex.Thesis.NeverNote.Shared.Data;
using JsonApiDotNetCore.Exceptions;
using JsonApiDotNetCore.Models.JsonApiDocuments;
using JsonApiDotNetCore.Services;

namespace Flogex.Thesis.NeverNote.Rest.Authors
{
    public class AuthorsService : IResourceService<Author>
    {
        private readonly AuthorsRepository _authors;
        private readonly NotesRepository _notes;

        public AuthorsService(AuthorsRepository authors, NotesRepository notes)
        {
            _authors = authors;
            _notes = notes;
        }

        public async Task<Author> CreateAsync(Author entity)
        {
            var input = new AuthorsRepository.AuthorInput(
                userName: entity.UserName,
                givenName: entity.GivenName,
                familyName: entity.FamilyName,
                email: entity.Email,
                birthDate: entity.BirthDate);

            var addedAuthor = await _authors.AddAuthor(input);
            return Author.FromAuthor(addedAuthor);
        }

        public async Task<IReadOnlyCollection<Author>> GetAsync()
            => (await _authors.GetAll()).Select(Author.FromAuthor).ToList().AsReadOnly();

        public async Task<Author> GetAsync(int id)
        {
            var result = await _authors.GetById(id);

            return result.HasValue
                ? Author.FromAuthor(result.Value)
                : null;
        }

        public async Task<object> GetSecondaryAsync(int id, string relationshipName)
        {
            if (relationshipName == "editedNotes")
            {
                var author = await _authors.GetById(id);

                if (!author.HasValue)
                    throw new JsonApiException(new Error(HttpStatusCode.NotFound));

                var results = await _notes.GetByAuthor(author.Value);
                return results.Select(Note.FromNote);
            }

            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task<Author> UpdateAsync(int id, Author resource) => throw new NotImplementedException();
        public Task UpdateRelationshipAsync(int id, string relationshipName, object relationships) => throw new NotImplementedException();
        Task<Author> IGetRelationshipService<Author, int>.GetRelationshipAsync(int id, string relationshipName) => throw new NotImplementedException();
    }
}
