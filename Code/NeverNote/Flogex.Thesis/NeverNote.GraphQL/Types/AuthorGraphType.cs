using System.Collections.Generic;
using GraphQL.Types;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Models;
using System.Threading.Tasks;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class AuthorGraphType : ObjectGraphType<Author>
    {
        public const string IdPrefix = "Author_";

        private readonly NotesRepository _notes;

        public AuthorGraphType(NotesRepository notes)
        {
            _notes = notes;

            this.Name = "Author";
            this.Description = "Represents someone who has manipulated a note.";

            Field<NonNullGraphType<IdGraphType>>(
                name: "id",
                description: "A globally unique identifier for the author.",
                resolve: _ => IdPrefix + _.Source.Id);

            Field<StringGraphType>(
                name: "userName",
                description: "The user name of the author.",
                resolve: _ => _.Source.UserName);

            Field<StringGraphType>(
                name: "fullName",
                description: "The fully qualified name of the author.",
                resolve: _ => GetFullName(_.Source));

            Field<StringGraphType>(
                name: "email",
                description: "The email address of the author.",
                resolve: _ => _.Source.Email);

            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<NoteGraphType>>>>(
                name: "editedNotes",
                description: "The notes this author either created or has contributed to.",
                resolve: async _ => await GetNotesManipulatedBy(_.Source));
        }

        private static string GetFullName(Author author)
            => author.GivenName + " " + author.FamilyName;

        private ValueTask<IEnumerable<Note>> GetNotesManipulatedBy(Author author)
            => _notes.GetByAuthor(author);
    }
}