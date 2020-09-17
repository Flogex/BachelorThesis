using GraphQL.Types;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class NoteGraphType : ObjectGraphType<Note>
    {
        public const string IdPrefix = "Note_";

        public NoteGraphType()
        {
            this.Name = "Note";
            this.Description = "Represents a note in the note-taking app.";

            Field<NonNullGraphType<IdGraphType>>(
                name: "id",
                description: "How the note can be identified.",
                resolve: _ => IdPrefix + _.Source.Id);

            Field<NonNullGraphType<StringGraphType>>(
                name: "title",
                description: "What the note is about.",
                resolve: _ => _.Source.Title);

            Field<NonNullGraphType<StringGraphType>>(
                name: "content",
                description: "What is in the note.",
                resolve: _ => _.Source.Content);

            Field<NonNullGraphType<DateGraphType>>(
                name: "dateCreated",
                description: "When the note was created.",
                resolve: _ => _.Source.DateCreated);

            Field<NonNullGraphType<AuthorGraphType>>(
                name: "creator",
                description: "Who is responsible for the note.",
                resolve: _ => _.Source.Creator);

            Field<NonNullGraphType<ListGraphType<NonNullGraphType<AuthorGraphType>>>>(
                name: "contributors",
                description: "Who else has edited the note.",
                resolve: _ => _.Source.Contributors);

            Field<NonNullGraphType<ListGraphType<NonNullGraphType<KeywordGraphType>>>>(
                name: "keywords",
                description: "What categories the note belongs to.",
                resolve: _ => _.Source.Keywords);
        }
    }
}
