using System;
using GraphQL.Types;
using GraphQL.Utilities;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class NeverNoteSchema : Schema
    {
        public NeverNoteSchema(IServiceProvider services) : base(services)
        {
            this.Query = services.GetRequiredService<NotesQuery>();
            this.Mutation = services.GetRequiredService<NotesMutation>();
            RegisterValueConverter(new KeywordAstValueConverter());
        }
    }
}
