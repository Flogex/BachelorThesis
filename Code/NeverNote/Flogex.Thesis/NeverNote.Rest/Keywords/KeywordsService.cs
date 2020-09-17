using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Rest.Notes;
using Flogex.Thesis.NeverNote.Shared.Data;
using JsonApiDotNetCore.Exceptions;
using JsonApiDotNetCore.Models.JsonApiDocuments;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Flogex.Thesis.NeverNote.Rest.Keywords
{
    public class KeywordsService : IGetAllService<Keyword, string>, ICreateService<Keyword, string>
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KeywordsService(NotesRepository notes, AuthorsRepository authors, IHttpContextAccessor httpContextAccessor)
        {
            _notes = notes;
            _authors = authors;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IReadOnlyCollection<Keyword>> GetAsync()
        {
            var noteId = int.Parse(_httpContextAccessor.HttpContext.GetRouteValue("noteId") as string);

            var note = await _notes.GetById(noteId);

            if (!note.HasValue)
                throw new JsonApiException(new Error(HttpStatusCode.NotFound));

            return note.Value.Keywords.Select(Keyword.FromKeyword).ToList().AsReadOnly();
        }

        public async Task<Keyword> CreateAsync(Keyword entity)
        {
            var context = _httpContextAccessor.HttpContext;
            var userName = context.GetUserName();

            if (userName is null)
                throw new JsonApiException(new Error(HttpStatusCode.Unauthorized));

            var editor = await _authors.GetByUserName(userName);

            if (!editor.HasValue)
                throw new JsonApiException(new Error(HttpStatusCode.Unauthorized));

            var noteId = int.Parse(context.GetRouteValue("noteId") as string);

            var found = await _notes.AddKeywordToNote(noteId, entity.Value, editor.Value);

            if (!found)
                throw new JsonApiException(new Error(HttpStatusCode.NotFound));

            return entity;
        }
    }
}
