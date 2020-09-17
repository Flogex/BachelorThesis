using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Data;
using Grpc.Core;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public class AddKeywordHandler : IRequestHandler<AddKeywordRequest, AddKeywordResponse>
    {
        private readonly NotesRepository _notes;
        private readonly AuthorsRepository _authors;

        public AddKeywordHandler(NotesRepository notes, AuthorsRepository authors)
        {
            _notes = notes;
            _authors = authors;
        }

        public async ValueTask<AddKeywordResponse> Handle(AddKeywordRequest request, ServerCallContext ctx)
        {
            var userName = ctx.GetUserName();
            var editor = await _authors.GetByUserName(userName, ctx.CancellationToken);

            if (!editor.HasValue)
                throw new UserNotFoundRpcException(userName);

            if (request.NoteId is null || request.NoteId.Value == default)
                throw new ArgumentNullRpcException("note_id");

            if (string.IsNullOrEmpty(request.NewKeyword))
                throw new ArgumentNullRpcException("new_keyword");

            var id = (int)request.NoteId.Value;
            var keyword = request.NewKeyword;
            var found = await _notes.AddKeywordToNote(id, keyword, editor.Value, ctx.CancellationToken);

            if (!found)
                throw new NotFoundRpcException("Note", id);

            var updatedNote = await _notes.GetById(id, ctx.CancellationToken);
            return new AddKeywordResponse() { UpdatedNote = ModelMapper.MapNote(updatedNote.Value) };
        }
    }
}
