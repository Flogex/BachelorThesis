using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Flogex.Thesis.NeverNote.Rest.Keywords
{
    [Route("notes/{noteId:int}/keywords")]
    [DisableRoutingConvention]
    [ResponseCache(CacheProfileName = "Cache60")]
    public class KeywordsController : JsonApiController<Keyword, string>
    {
        public KeywordsController(IJsonApiOptions options, ILoggerFactory loggerFactory,
            IGetAllService<Keyword, string> getAll, ICreateService<Keyword, string> create)
            : base(options, loggerFactory, getAll: getAll, create: create)
        { }
    }
}
