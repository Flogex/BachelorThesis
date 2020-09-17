using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Flogex.Thesis.NeverNote.Rest.Authors
{
    [ResponseCache(CacheProfileName = "Cache60")]
    public class AuthorsController : JsonApiController<Author>
    {
        public AuthorsController(IJsonApiOptions jsonApiOptions, ILoggerFactory loggerFactory, IResourceService<Author> service)
            : base(jsonApiOptions, loggerFactory, service)
        {
        }
    }
}
