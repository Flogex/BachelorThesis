using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Models.Annotation;

namespace Flogex.Thesis.NeverNote.Rest.Keywords
{
    public class Keyword : Identifiable<string>
    {
        [Key]
        [Attr]
        public string Value { get; set; }

        public static Keyword FromKeyword(Shared.Models.Keyword keyword)
        {
            return new Keyword
            {
                Value = keyword.Value
            };
        }
    }
}