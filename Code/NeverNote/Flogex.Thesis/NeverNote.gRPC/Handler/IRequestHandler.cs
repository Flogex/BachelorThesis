using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public interface IRequestHandler<TRequest, TResponse> where TRequest : IMessage where TResponse : IMessage
    {
        ValueTask<TResponse> Handle(TRequest request, ServerCallContext ctx);
    }
}
