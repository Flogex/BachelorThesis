using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Data;
using Grpc.Core;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public class DeleteNoteHandler : IRequestHandler<DeleteNoteRequest, DeleteNoteResponse>
    {
        private readonly NotesRepository _notes;

        public DeleteNoteHandler(NotesRepository notes)
        {
            _notes = notes;
        }

        public async ValueTask<DeleteNoteResponse> Handle(DeleteNoteRequest request, ServerCallContext ctx)
        {
            if (request.Id is null || request.Id.Value == default)
                throw new ArgumentNullRpcException("id");

            var id = (int)request.Id.Value;

            var found = await _notes.Remove(id, ctx.CancellationToken);

            if (!found)
                throw new NotFoundRpcException("Note", id);

            return new DeleteNoteResponse { Success = true };
        }
    }
}
