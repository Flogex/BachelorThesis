using System.Collections.Generic;
using System.Linq;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public static class Helper
    {
        public static IEnumerable<T> ToEnumerable<T>(this T? nullable) where T : struct
            => nullable.HasValue ? new[] { nullable.Value } : Enumerable.Empty<T>();

        public static string GetUserName(this ServerCallContext ctx)
        {
            var identity = ctx.GetHttpContext().User?.Identity;
            var userName = identity?.Name;

            if (identity?.IsAuthenticated != true || userName is null)
                throw new UnauthenticatedRpcException();

            return userName;
        }

        public static IServiceCollection RegisterRequestHandler(this IServiceCollection services)
        {
            return services
                .AddTransient<IRequestHandler<GetNotesRequest, GetNotesResponse>, GetNotesHandler>()
                .AddTransient<IRequestHandler<GetAuthorsRequest, GetAuthorsResponse>, GetAuthorsHandler>()
                .AddTransient<IRequestHandler<AddNoteRequest, AddNoteResponse>, AddNoteHandler>()
                .AddTransient<IRequestHandler<AddKeywordRequest, AddKeywordResponse>, AddKeywordHandler>()
                .AddTransient<IRequestHandler<DeleteNoteRequest, DeleteNoteResponse>, DeleteNoteHandler>()
                .AddTransient<IRequestHandler<SignupRequest, SignupResponse>, SignupHandler>();
        }
    }
}