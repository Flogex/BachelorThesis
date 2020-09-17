using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Data;
using Grpc.Core;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public class SignupHandler : IRequestHandler<SignupRequest, SignupResponse>
    {
        private readonly AuthorsRepository _authors;

        public SignupHandler(AuthorsRepository authors)
        {
            _authors = authors;
        }

        public async ValueTask<SignupResponse> Handle(SignupRequest request, ServerCallContext ctx)
        {
            if (string.IsNullOrEmpty(request.UserName))
                throw new ArgumentNullRpcException("user_name");

            if (string.IsNullOrEmpty(request.GivenName))
                throw new ArgumentNullRpcException("given_name");

            if (string.IsNullOrEmpty(request.FamilyName))
                throw new ArgumentNullRpcException("famly_name");

            if (string.IsNullOrEmpty(request.Email))
                throw new ArgumentNullRpcException("email");

            var input = new AuthorsRepository.AuthorInput(
                userName: request.UserName,
                givenName: request.GivenName,
                familyName: request.FamilyName,
                email: request.Email,
                birthDate: request.Birthdate?.ToDateTime());

            var createdAuthor = await _authors.AddAuthor(input, ctx.CancellationToken);

            return new SignupResponse() { CreatedAuthor = ModelMapper.MapAuthor(createdAuthor) };
        }
    }
}
