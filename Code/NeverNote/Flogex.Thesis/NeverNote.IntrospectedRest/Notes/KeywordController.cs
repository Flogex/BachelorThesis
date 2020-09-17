using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.NeverNote.Shared.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Notes
{
    [Route("/notes/{id}/keywords")]
    public class KeywordController : Controller
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CancellationToken _cancellation => _httpContextAccessor.HttpContext.RequestAborted;

        public KeywordController(NotesRepository notes, AuthorsRepository authors, IHttpContextAccessor httpContextAccessor)
        {
            _notes = notes;
            _authors = authors;
            _httpContextAccessor = httpContextAccessor;
        }

        [RestGet]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(KeywordsDTO))]
        [ResponseCache(CacheProfileName = "Cache60")]
        public async Task<IActionResult> GetKeywordsForNote(int id)
        {
            var note = await _notes.GetById(id, _cancellation);

            if (!note.HasValue)
                return NotFound();

            return new RestResult(note.Value.Keywords.Select(_ => _.Value));
        }

        [HttpPost]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(IEnumerable<string>))]
        public async Task<IActionResult> AddKeywordToNote(int id, [FromForm]string keyword)
        {
            if (keyword is null)
                return BadRequest();

            var userName = this.HttpContext.GetUserName();

            if (userName is null)
                return Unauthorized();

            var editor = await _authors.GetByUserName(userName, _cancellation);

            if (!editor.HasValue)
                return Unauthorized();

            var exists = await _notes.AddKeywordToNote(id, keyword, editor.Value, _cancellation);

            if (!exists)
                return NotFound();

            var note = await _notes.GetById(id, _cancellation);

            return new RestResult(note.Value.Keywords.Select(_ => _.Value));
        }
    }
}
