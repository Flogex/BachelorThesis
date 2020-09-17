using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Data;
using Grpc.Core;
using static Flogex.Thesis.NeverNote.gRPC.GetAuthorsRequest.SearchFieldOneofCase;
using Models = Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public class GetAuthorsHandler : IRequestHandler<GetAuthorsRequest, GetAuthorsResponse>
    {
        private readonly AuthorsRepository _authors;

        public GetAuthorsHandler(AuthorsRepository authors)
        {
            _authors = authors;
        }

        public async ValueTask<GetAuthorsResponse> Handle(GetAuthorsRequest request, ServerCallContext ctx)
        {
            var searchTask = request.SearchFieldCase switch
            {
                None => GetAll(ctx),
                Id => GetAuthorById(request.Id, ctx),
                _ => throw new NotImplementedRpcException()
            };

            var results = await searchTask;
            var response = new GetAuthorsResponse();
            response.Authors.Add(results.Select(ModelMapper.MapAuthor));
            return response;
        }

        private ValueTask<IEnumerable<Models.Author>> GetAll(ServerCallContext ctx)
            => _authors.GetAll(ctx.CancellationToken);

        private async ValueTask<IEnumerable<Models.Author>> GetAuthorById(Author.Types.Id id, ServerCallContext ctx)
        {
            var result = await _authors.GetById((int)id.Value, ctx.CancellationToken);
            return result.ToEnumerable();
        }
    }
}
