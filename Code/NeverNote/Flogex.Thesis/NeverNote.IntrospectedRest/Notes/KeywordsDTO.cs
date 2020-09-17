using System.Collections;
using System.Collections.Generic;
using Flogex.Thesis.JsonHyperSchema.Attributes;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Notes
{
    [Link("add", "/notes/{id}/keywords", new [] { "id" })]
    public class KeywordsDTO : IEnumerable<string>
    {
        private readonly IEnumerable<string> _source;

        public KeywordsDTO(IEnumerable<string> source)
        {
            _source = source;
        }

        public IEnumerator<string> GetEnumerator()
            => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _source.GetEnumerator();
    }
}
