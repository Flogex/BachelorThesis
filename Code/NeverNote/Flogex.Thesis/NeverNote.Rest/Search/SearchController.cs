using System.Linq;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Rest.Notes;
using Flogex.Thesis.NeverNote.Shared.Data;
using JsonApiDotNetCore.Internal.Contracts;
using JsonApiDotNetCore.RequestServices;
using JsonApiDotNetCore.RequestServices.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Flogex.Thesis.NeverNote.Rest.Search
{
    [Route("/search")]
    [ResponseCache(CacheProfileName = "Cache60")]
    public class SearchController : Controller
    {
        private readonly NotesRepository _notes;
        private readonly CurrentRequest _currentRequest;
        private readonly IResourceGraph _resourceGraph;

        public SearchController(NotesRepository notes, ICurrentRequest currentRequest, IResourceGraph resourceGraph)
        {
            _notes = notes;
            _currentRequest = currentRequest as CurrentRequest;
            _resourceGraph = resourceGraph;
        }

        [HttpGet]
        public IActionResult GetAsync()
        {
            var payload = new
            {
                data = new
                {
                    relationships = new
                    {
                        byKeyword = new
                        {
                            links = new
                            {
                                related = "/search/byKeyword"
                            }
                        }
                    },
                    links = new
                    {
                        self = "/search"
                    }
                }
            };

            return Ok(payload);
        }

        [HttpGet]
        [Route("byKeyword")]
        public async Task<IActionResult> SearchByKeyword([FromQuery]string keyword)
        {
            var notes = await _notes.GetByKeyword(keyword);
            var results = notes.Select(Note.FromNote);

            _currentRequest.IsReadOnly = true;
            _currentRequest.IsCollection = true;
            _currentRequest.Kind = EndpointKind.Primary;
            _currentRequest.PrimaryResource = _resourceGraph.GetResourceContext<Note>();
            this.HttpContext.Items["IsJsonApiRequest"] = bool.TrueString;

            return Ok(results);
        }
    }
}
