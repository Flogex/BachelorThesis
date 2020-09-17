using System.Collections.Generic;
using System.Security.Claims;

namespace Flogex.Thesis.NeverNote.GraphQL
{
    public class GraphQlUserContext : Dictionary<string, object>
    {
        private const string userKey = "$user";

        public ClaimsPrincipal? User
        {
            get => this.GetValueOrDefault(userKey) as ClaimsPrincipal;
            set
            {
                this.Remove(userKey);
#pragma warning disable CS8604 // Possible null reference argument.
                this.Add(userKey, value);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }
    }
}
