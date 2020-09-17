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
    [Route("/notes")]
    public class NotesController : Controller
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CancellationToken _cancellation => _httpContextAccessor.HttpContext.RequestAborted;

        public NotesController(NotesRepository notes, AuthorsRepository authors, IHttpContextAccessor httpContextAccessor)
        {
            _notes = notes;
            _authors = authors;
            _httpContextAccessor = httpContextAccessor;
        }

        [RestGet]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(NotesOverviewDTO))]
        [ResponseCache(CacheProfileName = "Cache60")]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _notes.GetAll(_cancellation);
            return new RestResult(notes.Select(NoteOverviewDTO.FromNote));
        }

        [RestGet]
        [Route("{id}")]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(NoteDTO))]
        [ResponseCache(CacheProfileName = "Cache60")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = await _notes.GetById(id, _cancellation);

            if (!note.HasValue)
                return NotFound();

            return new RestResult(NoteDTO.FromNote(note.Value));
        }

        [HttpPost]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(NoteDTO))]
        public async Task<IActionResult> AddNote([FromBody]string? title, [FromBody] string? content)
        {
            var userName = this.HttpContext.GetUserName();

            if (userName is null)
                return Unauthorized();

            var creator = await _authors.GetByUserName(userName, _cancellation);

            if (!creator.HasValue)
                return Unauthorized();

            if (title is null || content is null)
                return BadRequest();

            var input = new NotesRepository.NoteInput(title, content);

            var note = await _notes.AddNote(input, creator.Value, _cancellation);

            return new RestResult(NoteDTO.FromNote(note));
        }

        [HttpDelete]
        [Route("{id}")]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(NoteDTO))]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var removed = await _notes.Remove(id);

            if (!removed)
                return NotFound();

            return NoContent();
        }
    }
}
