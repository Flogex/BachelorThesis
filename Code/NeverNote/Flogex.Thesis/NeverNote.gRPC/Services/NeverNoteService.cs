using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.gRPC.Handler;
using Grpc.Core;

namespace Flogex.Thesis.NeverNote.gRPC
{
    public class NeverNoteServiceImpl : NeverNoteService.NeverNoteServiceBase
    {
        private readonly IRequestHandler<GetNotesRequest, GetNotesResponse> _getNotesHandler;
        private readonly IRequestHandler<GetAuthorsRequest, GetAuthorsResponse> _getAuthorsHandler;
        private readonly IRequestHandler<AddNoteRequest, AddNoteResponse> _addNoteHandler;
        private readonly IRequestHandler<AddKeywordRequest, AddKeywordResponse> _addKeywordHandler;
        private readonly IRequestHandler<DeleteNoteRequest, DeleteNoteResponse> _deleteNoteHandler;
        private readonly IRequestHandler<SignupRequest, SignupResponse> _signupHandler;

        public NeverNoteServiceImpl(
            IRequestHandler<GetNotesRequest, GetNotesResponse> getNotesHandler,
            IRequestHandler<GetAuthorsRequest, GetAuthorsResponse> getAuthorsHandler,
            IRequestHandler<AddNoteRequest, AddNoteResponse> addNoteHandler,
            IRequestHandler<AddKeywordRequest, AddKeywordResponse> addKeywordHandler,
            IRequestHandler<DeleteNoteRequest, DeleteNoteResponse> deleteNoteHandler,
            IRequestHandler<SignupRequest, SignupResponse> signupHandler)
        {
            _getNotesHandler = getNotesHandler;
            _getAuthorsHandler = getAuthorsHandler;
            _addNoteHandler = addNoteHandler;
            _addKeywordHandler = addKeywordHandler;
            _deleteNoteHandler = deleteNoteHandler;
            _signupHandler = signupHandler;
        }

        public override Task<GetNotesResponse> GetNotes(GetNotesRequest request, ServerCallContext context)
            => _getNotesHandler.Handle(request, context).AsTask();

        public override Task<GetAuthorsResponse> GetAuthors(GetAuthorsRequest request, ServerCallContext context)
            => _getAuthorsHandler.Handle(request, context).AsTask();

        public override Task<AddNoteResponse> AddNote(AddNoteRequest request, ServerCallContext context)
            => _addNoteHandler.Handle(request, context).AsTask();

        public override Task<AddKeywordResponse> AddKeyword(AddKeywordRequest request, ServerCallContext context)
            => _addKeywordHandler.Handle(request, context).AsTask();

        public override Task<DeleteNoteResponse> DeleteNote(DeleteNoteRequest request, ServerCallContext context)
            => _deleteNoteHandler.Handle(request, context).AsTask();

        public override Task<SignupResponse> Signup(SignupRequest request, ServerCallContext context)
            => _signupHandler.Handle(request, context).AsTask();
    }
}
