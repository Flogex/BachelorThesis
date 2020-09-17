using GraphQL.Types;
using Flogex.Thesis.NeverNote.Shared.Data;
using Flogex.Thesis.NeverNote.Shared.Models;
using Flogex.Thesis.NeverNote.GraphQL.Helper;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class NotesQuery : ObjectGraphType
    {
        public NotesQuery(NotesRepository notes, AuthorsRepository authors)
        {
            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<NoteGraphType>>>>(
                name: "notes",
                description: "Returns all existing notes.",
                resolve: async ctx => await notes.GetAll(ctx.CancellationToken));

            FieldAsync<NoteGraphType>(
                name: "noteById",
                description: "Returns the note identified by the given id, if present.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "id",
                        Description = "The id of the note you are looking for."
                    }),
                resolve: async ctx =>
                {
                    var prefixedId = ctx.GetRequiredArgument<string>("id");
                    var id = IdParser.ParsePrefixedId(prefixedId, NoteGraphType.IdPrefix);
                    return await notes.GetById(id, ctx.CancellationToken);
                });

            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<NoteGraphType>>>>(
                name: "notesByTitle",
                description: "Returns all notes whose titles start with the given prefix.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>>()
                    {
                        Name = "title",
                        Description = "A prefix of the title of the notes you are looking for."
                    }),
                resolve: async ctx =>
                {
                    var titlePrefix = ctx.GetRequiredArgument<string>("title");
                    return await notes.GetByTitle(titlePrefix, ctx.CancellationToken);
                });

            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<NoteGraphType>>>>(
                name: "notesByAuthor",
                description: "Returns all notes manipulated by the given author.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "authorId",
                        Description = "The id of the author whose notes you are looking for."
                    }),
                resolve: async ctx =>
                {
                    var prefixedId = ctx.GetRequiredArgument<string>("authorId");
                    var authorId = IdParser.ParsePrefixedId(prefixedId, AuthorGraphType.IdPrefix);
                    var author = await authors.GetById(authorId, ctx.CancellationToken);

                    if (!author.HasValue)
                        return Errors.InvalidArgumentException<object>("authorId", "Author could not be found");

                    return notes.GetByAuthor(author.Value, ctx.CancellationToken);
                });

            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<NoteGraphType>>>>(
                name: "notesForKeyword",
                description: "Returns all notes with the given keyword.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<KeywordGraphType>>()
                    {
                        Name = "keyword",
                        Description = "The keyword of the notes your are looking for."
                    }),
                resolve: async ctx =>
                {
                    var keyword = ctx.GetRequiredArgument<Keyword>("keyword");
                    return await notes.GetByKeyword(keyword, ctx.CancellationToken);
                });

            FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<AuthorGraphType>>>>(
                name: "authors",
                description: "Returns all existing authors.",
                resolve: async ctx => await authors.GetAll(ctx.CancellationToken));

            FieldAsync<AuthorGraphType>(
                name: "authorById",
                description: "Returns the author identified by the given id, if present.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "id",
                        Description = "The id of the author you are looking for."
                    }),
                resolve: async ctx =>
                {
                    var prefixedId = ctx.GetRequiredArgument<string>("id");
                    var id = IdParser.ParsePrefixedId(prefixedId, AuthorGraphType.IdPrefix);
                    return await authors.GetById(id, ctx.CancellationToken);
                });
        }
    }
}
