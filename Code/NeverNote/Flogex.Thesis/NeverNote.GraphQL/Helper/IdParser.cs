using System;

namespace Flogex.Thesis.NeverNote.GraphQL.Helper
{
    internal static class IdParser
    {
        public static int ParsePrefixedId(ReadOnlySpan<char> prefixedId, ReadOnlySpan<char> prefix)
        {
            var prefixPosition = prefixedId.IndexOf(prefix);

            if (prefixPosition == 0 && int.TryParse(prefixedId.Slice(prefix.Length), out var id))
                return id;

            return Errors.IdParsingError<int>(prefix.ToString());
        }
    }
}
