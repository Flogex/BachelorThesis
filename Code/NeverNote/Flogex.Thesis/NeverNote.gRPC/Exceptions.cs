using Grpc.Core;
using static Grpc.Core.StatusCode;

namespace Flogex.Thesis.NeverNote.gRPC
{
    public class InvalidArgumentRpcException : RpcException
    {
        public InvalidArgumentRpcException(string message)
            : this(message, Metadata.Empty) { }

        public InvalidArgumentRpcException(string message, Metadata trailers)
            : base(new Status(InvalidArgument, message), trailers) { }
    }

    public class ArgumentNullRpcException : RpcException
    {
        public ArgumentNullRpcException(string argument)
            : this(argument, Metadata.Empty) { }

        public ArgumentNullRpcException(string argument, Metadata trailers)
            : base(new Status(InvalidArgument, $"Argument {argument} must not be null."), trailers) { }
    }

    public class UnauthenticatedRpcException : RpcException
    {
        public UnauthenticatedRpcException()
            : base(new Status(Unauthenticated, "You must authenticate using Basic authentication.")) { }
    }

    public class UserNotFoundRpcException : RpcException
    {
        public UserNotFoundRpcException(string userName)
            : base(new Status(PermissionDenied, $"No user with name ${userName} exists. Please sign up.")) { }
    }

    public class NotImplementedRpcException : RpcException
    {
        public NotImplementedRpcException()
            : base(new Status(Unimplemented, "The functionality is not implemented yet.")) { }
    }

    public class NotFoundRpcException : RpcException
    {
        public NotFoundRpcException(string entityType, int id)
            : base(new Status(NotFound, $"{entityType} with id {id} could not be found.")) { }
    }
}
