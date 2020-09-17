using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Data;
using Grpc.Core;
using static Flogex.Thesis.NeverNote.gRPC.GetNotesRequest.SearchFieldOneofCase;
using Models = Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public class GetNotesHandler : IRequestHandler<GetNotesRequest, GetNotesResponse>
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;

        public GetNotesHandler(NotesRepository notes, AuthorsRepository authors)
        {
            _notes = notes;
            _authors = authors;
        }

        public async ValueTask<GetNotesResponse> Handle(GetNotesRequest request, ServerCallContext ctx)
        {
            var searchTask = request.SearchFieldCase switch
            {
                None => GetAllNotes(ctx),
                Id => GetNotesById(request.Id, ctx),
                Title => GetNotesByTitle(request.Title, ctx),
                AuthorId => GetNotesByAuthor(request.AuthorId, ctx),
                Keyword => GetNotesByKeyword(request.Keyword, ctx),
                _ => throw new NotImplementedRpcException()
            };

            var results = await searchTask;
            var response = new GetNotesResponse();
            response.Notes.Add(results.Select(ModelMapper.MapNote));
            return response;
        }

        private ValueTask<IEnumerable<Models.Note>> GetAllNotes(ServerCallContext ctx)
            => _notes.GetAll(ctx.CancellationToken);

        private async ValueTask<IEnumerable<Models.Note>> GetNotesById(Note.Types.Id id, ServerCallContext ctx)
        {
            var result = await _notes.GetById((int)id.Value, ctx.CancellationToken);
            return result.ToEnumerable();
        }

        private ValueTask<IEnumerable<Models.Note>> GetNotesByTitle(string title, ServerCallContext ctx)
            => _notes.GetByTitle(title, ctx.CancellationToken);

        private async ValueTask<IEnumerable<Models.Note>> GetNotesByAuthor(Author.Types.Id authorId, ServerCallContext ctx)
        {
            if (authorId is null)
                throw new ArgumentNullRpcException(nameof(authorId));

            var author = await _authors.GetById((int)authorId.Value, ctx.CancellationToken);

            if (!author.HasValue)
                throw new InvalidArgumentRpcException($"The author with id {authorId.Value} could not be found");

            return await _notes.GetByAuthor(author.Value, ctx.CancellationToken);
        }

        private ValueTask<IEnumerable<Models.Note>> GetNotesByKeyword(string keyword, ServerCallContext ctx)
            => _notes.GetByKeyword(keyword, ctx.CancellationToken);
    }
}
