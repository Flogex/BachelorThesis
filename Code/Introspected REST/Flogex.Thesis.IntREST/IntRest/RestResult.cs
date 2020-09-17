using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Flogex.Thesis.IntRest
{
    public class RestResult : IActionResult
    {
        private readonly List<IRuntimeMicrotype> _microtypes = new List<IRuntimeMicrotype>();

        public RestResult(object data)
        {
            this.Data = data;
        }

        public object Data { get; }

        public int StatusCode { get; set; } = 200;

        public IEnumerable<IRuntimeMicrotype> RuntimeMicrotypes => _microtypes;

        public RestResult AddRuntimeMicrotype(IRuntimeMicrotype microtype)
        {
            _microtypes.Add(microtype);
            return this;
        }

        public RestResult RemoveRuntimeMicrotype(IRuntimeMicrotype microtype)
        {
            _microtypes.Remove(microtype);
            return this;
        }

        Task IActionResult.ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var services = context.HttpContext.RequestServices;
            var executor = services.GetRequiredService<IActionResultExecutor<RestResult>>();
            return executor.ExecuteAsync(context, this);
        }
    }
}
