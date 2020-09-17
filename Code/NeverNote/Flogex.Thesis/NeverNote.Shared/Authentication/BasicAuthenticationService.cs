using System.Threading;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Data;

namespace Flogex.Thesis.NeverNote.Shared.Authentication
{
    public class BasicAuthenticationService
    {
        private readonly AuthorsRepository _authors;

        public BasicAuthenticationService(AuthorsRepository authors)
        {
            _authors = authors;
        }

        public async Task<bool> IsValidUserAsync(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _authors.GetByUserName(userName, cancellationToken);
            return user != default;
        }
    }
}
