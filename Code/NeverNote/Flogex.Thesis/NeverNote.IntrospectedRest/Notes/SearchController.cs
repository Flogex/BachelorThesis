using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Notes
{
    [Route("/notes/search")]
    public class SearchController : Controller
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CancellationToken _cancellation => _httpContextAccessor.HttpContext.RequestAborted;

        public SearchController(NotesRepository notes, AuthorsRepository authors, IHttpContextAccessor httpContextAccessor)
        {
            _notes = notes;
            _authors = authors;
            _httpContextAccessor = httpContextAccessor;
        }

        [RestGet]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(SearchOverviewDTO))]
        [ResponseCache(CacheProfileName = "Cache60")]
        public Task<IActionResult> GetOverview()
        {
            IActionResult result = new RestResult(new SearchOverviewDTO());
            return Task.FromResult(result);
        }

        [Route("byTitle")]
        [RestGet]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(NotesOverviewDTO))]
        public async Task<IActionResult> SearchByTitle([FromQuery]string? title)
        {
            if (title is null)
                return BadRequest();

            var notes = await _notes.GetByTitle(title, _cancellation);
            return new RestResult(notes.Select(NoteOverviewDTO.FromNote));
        }

        [Route("byKeyword")]
        [RestGet]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(NotesOverviewDTO))]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string? keyword)
        {
            if (keyword is null)
                return BadRequest();

            var notes = await _notes.GetByKeyword(keyword, _cancellation);
            return new RestResult(notes.Select(NoteOverviewDTO.FromNote));
        }

        [Route("byAuthor")]
        [RestGet]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(NotesOverviewDTO))]
        public async Task<IActionResult> SearchByAuthor([FromQuery] int? authorId)
        {
            if (!authorId.HasValue)
                return BadRequest();

            var author = await _authors.GetById(authorId.Value, _cancellation);

            var notes = author.HasValue
                ? await _notes.GetByAuthor(author.Value, _cancellation)
                : Enumerable.Empty<Note>();

            return new RestResult(notes.Select(NoteOverviewDTO.FromNote));
        }
    }
}
