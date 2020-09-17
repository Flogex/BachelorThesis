using Microsoft.AspNetCore.Http;

namespace Flogex.Thesis.NeverNote.IntrospectedRest.Notes
{
    internal static class HttpContextExtensions
    {
        public static string? GetUserName(this HttpContext context)
        {
            var identity = context.User?.Identity;
            var userName = identity?.Name;

            if (identity?.IsAuthenticated == true)
                return userName;

            return null;
        }
    }
}
