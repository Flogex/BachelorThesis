using System;
using GraphQL;
using GraphQL.Types;

namespace Flogex.Thesis.NeverNote.GraphQL.Helper
{
    internal static class ResolveFieldContextExtensions
    {
        public static T GetRequiredArgument<T>(this IResolveFieldContext context, string name) where T : IEquatable<T>
        {
            var arg = context.GetArgument<T>(name);

            return arg.Equals(default)
                ? Errors.ArgumentMissingError<T>(name)
                : arg;
        }

        public static string RequireUserName(this IResolveFieldContext context)
        {
            var userContext = context.UserContext as GraphQlUserContext;
            var identity = userContext?.User?.Identity;
            var userName = identity?.Name;

            if (identity?.IsAuthenticated != true || userName is null)
                return Errors.AuthenticationRequiredError<string>();

            return userName;
        }
    }
}
