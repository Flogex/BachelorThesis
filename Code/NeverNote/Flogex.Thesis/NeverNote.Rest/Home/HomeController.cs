using Microsoft.AspNetCore.Mvc;

namespace Flogex.Thesis.NeverNote.Rest.Home
{
    [Route("/")]
    [ResponseCache(CacheProfileName = "Cache60")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult GetAsync()
        {
            var payload = new
            {
                data = new
                {
                    attributes = new
                    {
                        title = "NeverNote API"
                    },
                    relationships = new
                    {
                        notes = new
                        {
                            links = new
                            {
                                related = "/notes"
                            }
                        },
                        authors = new
                        {
                            links = new
                            {
                                related = "/authors"
                            }
                        },
                        search = new
                        {
                            links = new
                            {
                                related = "/search"
                            }
                        }
                    },
                    links = new
                    {
                        self = "/"
                    }
                }
            };

            return Ok(payload);
        }
    }
}
