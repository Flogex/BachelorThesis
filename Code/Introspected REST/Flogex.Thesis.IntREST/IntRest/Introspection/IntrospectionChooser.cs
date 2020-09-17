using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flogex.Thesis.IntRest.Configuration;
using Flogex.Thesis.IntRest.Conneg;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Flogex.Thesis.IntRest.Introspection
{
    public class IntrospectionChooser : IIntrospectionExecutor
    {
        private readonly RestOptions _options;

        public IntrospectionChooser(RestOptions options)
        {
            _options = options;
        }

        public Task ExecuteAsync(ControllerActionDescriptor action, HttpContext context)
        {
            IIntrospectionMicrotype? selectedMicrotype = new OverviewIntrospection(StatusCodes.Status200OK, _options);

            if (context.Items.TryGetValue(MicrotypeCollection.Identifier, out var value) &&
                value is MicrotypeCollection acceptedMicroTypes)
            {
                var introspectionTypes = acceptedMicroTypes
                    .Where(_ => _.Category == "introspection" && !string.IsNullOrEmpty(_.Identifier))
                    .Select(_ => _.Identifier);

                if (introspectionTypes.Any())
                {
                    selectedMicrotype = GetSelectedMicrotype(introspectionTypes)
                        ?? new OverviewIntrospection(StatusCodes.Status406NotAcceptable, _options);
                }
            }

            return selectedMicrotype.ExecuteAsync(action, context);
        }

        private IIntrospectionMicrotype? GetSelectedMicrotype(IEnumerable<string> acceptedMicrotypes)
        {
            var registeredMicrotypes = _options.Microtypes.IntrospectionMicrotypes.ToDictionary(m => m.Identifier);

            foreach (var identifier in acceptedMicrotypes)
                if (registeredMicrotypes.TryGetValue(identifier, out var negotiatedMicrotype))
                    return negotiatedMicrotype;

            return null;
        }
    }
}
