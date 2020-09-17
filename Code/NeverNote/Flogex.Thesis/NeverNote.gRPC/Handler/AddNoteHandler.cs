using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Data;
using Grpc.Core;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public class AddNoteHandler : IRequestHandler<AddNoteRequest, AddNoteResponse>
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;

        public AddNoteHandler(NotesRepository notes, AuthorsRepository authors)
        {
            _notes = notes;
            _authors = authors;
        }

        public async ValueTask<AddNoteResponse> Handle(AddNoteRequest request, ServerCallContext ctx)
        {
            var userName = ctx.GetUserName();
            var creator = await _authors.GetByUserName(userName, ctx.CancellationToken);

            if (!creator.HasValue)
                throw new UserNotFoundRpcException(userName);

            if (string.IsNullOrEmpty(request.Title))
                throw new ArgumentNullRpcException("title");

            if (string.IsNullOrEmpty(request.Content))
                throw new ArgumentNullRpcException("content");

            var input = new NotesRepository.NoteInput(request.Title, request.Content);
            var newNote = await _notes.AddNote(input, creator.Value, ctx.CancellationToken);

            return new AddNoteResponse { CreatedNote = ModelMapper.MapNote(newNote) };
        }
    }
}
