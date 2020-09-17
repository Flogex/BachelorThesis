using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest;
using Flogex.Thesis.IntRest.Attributes;
using Flogex.Thesis.NeverNote.Shared.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Authors
{
    [Route("/authors")]
    public class AuthorsController : Controller
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CancellationToken _cancellation => _httpContextAccessor.HttpContext.RequestAborted;

        public AuthorsController(NotesRepository notes, AuthorsRepository authors, IHttpContextAccessor httpContextAccessor)
        {
            _notes = notes;
            _authors = authors;
            _httpContextAccessor = httpContextAccessor;
        }

        [RestGet]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(IEnumerable<AuthorOverviewDTO>))]
        [ResponseCache(CacheProfileName = "Cache60")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authors.GetAll(_cancellation);
            return new RestResult(authors.Select(AuthorOverviewDTO.FromAuthor));
        }

        [RestGet]
        [Route("{id}")]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(AuthorDTO))]
        [ResponseCache(CacheProfileName = "Cache60")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authors.GetById(id, _cancellation);

            if (!author.HasValue)
                return NotFound();

            var editedNotes = await _notes.GetByAuthor(author.Value, _cancellation);

            return new RestResult(AuthorDTO.FromAuthor(author.Value, editedNotes));
        }

        [RestPost]
        [SupportsContentMicrotype("json")]
        [Returns(typeof(AuthorDTO))]
        public async Task<IActionResult> Signup([FromBody]SignupDTO newAuthor)
        {
            if (newAuthor.UserName is null || newAuthor.GivenName is null ||
                newAuthor.FamilyName is null || newAuthor.Email is null)
                return BadRequest();

            var input = new AuthorsRepository.AuthorInput(
                userName: newAuthor.UserName,
                givenName: newAuthor.GivenName,
                familyName: newAuthor.FamilyName,
                email: newAuthor.Email,
                birthDate: newAuthor.BirthDate);

            var author = await _authors.AddAuthor(input, _cancellation);

            var editedNotes = Enumerable.Empty<Shared.Models.Note>();

            return new RestResult(AuthorDTO.FromAuthor(author, editedNotes));
        }
    }
}
