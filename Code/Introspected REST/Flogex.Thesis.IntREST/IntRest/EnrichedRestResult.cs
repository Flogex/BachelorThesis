using System;
using System.Collections.Generic;
using System.Linq;
using Flogex.Thesis.IntRest.Content;
using Flogex.Thesis.IntRest.Runtime;

namespace Flogex.Thesis.IntRest
{
    internal class EnrichedRestResult
    {
        public EnrichedRestResult(IContentMicrotype contentMicrotype, IEnumerable<IRuntimeMicrotype> runtimeMicrotypes)
        {
            this.ContentMicrotype = contentMicrotype ?? throw new ArgumentNullException(nameof(contentMicrotype));
            this.RuntimeMicrotypes = runtimeMicrotypes ?? Enumerable.Empty<IRuntimeMicrotype>();
        }

        public IContentMicrotype ContentMicrotype { get; }

        public IEnumerable<IRuntimeMicrotype> RuntimeMicrotypes { get; }
    }
}
