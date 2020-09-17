using Flogex.Thesis.NeverNote.Shared.Data;
using GraphQL.Types;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class AuthorInputType : InputObjectGraphType<AuthorsRepository.AuthorInput>
    {
        public AuthorInputType()
        {
            this.Name = "AuthorInput";
            this.Description = "InputType for adding a new author.";

            Field(x => x.UserName);
            Field(x => x.GivenName);
            Field(x => x.FamilyName);
            Field(x => x.Email);
            Field(x => x.BirthDate, nullable: true);
        }
    }
}
