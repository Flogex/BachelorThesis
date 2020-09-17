using GraphQL.Types;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Models;
using Flogex.Thesis.NeverNote.GraphQL.Helper;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class NotesMutation : ObjectGraphType
    {
        public NotesMutation(NotesRepository notes, AuthorsRepository authors)
        {
            FieldAsync<NonNullGraphType<NoteGraphType>>(
                name: "addNote",
                description: "Add a new note.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<NoteInputType>>()
                    {
                        Name = "note",
                        Description = "The new note to be added."
                    }),
                resolve: async ctx =>
                {
                    var userName = ctx.RequireUserName();
                    var creator = await authors.GetByUserName(userName, ctx.CancellationToken);

                    if (!creator.HasValue)
                        return Errors.InvalidUsernameError<object>();

                    var newNote = ctx.GetRequiredArgument<NotesRepository.NoteInput>("note");

                    return await notes.AddNote(newNote, creator.Value, ctx.CancellationToken);
                });

            FieldAsync<NoteGraphType>(
                name: "addKeyword",
                description: "Add a new keyword to an existing note with the given id.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "noteId",
                        Description = "The id of the note the keyword should be added to."
                    },
                    new QueryArgument<NonNullGraphType<KeywordGraphType>>()
                    {
                        Name = "keyword",
                        Description = "The keyword to add to the note."
                    }),
                resolve: async ctx =>
                {
                    var prefixedId = ctx.GetRequiredArgument<string>("noteId");
                    var id = IdParser.ParsePrefixedId(prefixedId, NoteGraphType.IdPrefix);

                    var keyword = ctx.GetRequiredArgument<Keyword>("keyword");

                    var userName = ctx.RequireUserName();
                    var editor = await authors.GetByUserName(userName, ctx.CancellationToken);

                    if (!editor.HasValue)
                        return Errors.InvalidUsernameError<object>();

                    var found = await notes.AddKeywordToNote(id, keyword, editor.Value, ctx.CancellationToken);

                    if (!found)
                        return Errors.NotFound<object>("Note", prefixedId);

                    return await notes.GetById(id, ctx.CancellationToken);
                });

            FieldAsync<BooleanGraphType>(
                name: "deleteNote",
                description: "Deletes the note with the given id, if present.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "noteId",
                        Description = "The id of the note that should be deleted."
                    }),
                resolve: async ctx =>
                {
                    var prefixedId = ctx.GetRequiredArgument<string>("noteId");
                    var id = IdParser.ParsePrefixedId(prefixedId, NoteGraphType.IdPrefix);

                    var found = await notes.Remove(id, ctx.CancellationToken);

                    if (!found)
                        return Errors.NotFound<bool>("Note", prefixedId);

                    return true;
                });

            FieldAsync<NonNullGraphType<AuthorGraphType>>(
                name: "signup",
                description: "Create a new author.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AuthorInputType>>()
                    {
                        Name = "author",
                        Description = "The new author to be added."
                    }),
                resolve: async ctx =>
                {
                    var newAuthor = ctx.GetRequiredArgument<AuthorsRepository.AuthorInput>("author");
                    return await authors.AddAuthor(newAuthor, ctx.CancellationToken);
                });
        }
    }
}
