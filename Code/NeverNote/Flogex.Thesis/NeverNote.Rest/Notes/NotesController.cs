using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Flogex.Thesis.NeverNote.Rest.Notes
{
    [ResponseCache(CacheProfileName = "Cache60")]
    public class NotesController : JsonApiController<Note>
    {
        public NotesController(IJsonApiOptions options, ILoggerFactory loggerFactory, IResourceService<Note> service)
            : base(options, loggerFactory, service)
        { }
    }
}
