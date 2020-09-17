using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Flogex.Thesis.IntRest.Introspection
{
    public interface IIntrospectionExecutor
    {
        Task ExecuteAsync(ControllerActionDescriptor action, HttpContext context);
    }
}
