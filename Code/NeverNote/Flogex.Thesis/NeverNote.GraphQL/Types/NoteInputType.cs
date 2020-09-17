using GraphQL.Types;
using Flogex.Thesis.NeverNote.Shared.Data;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class NoteInputType : InputObjectGraphType<NotesRepository.NoteInput>
    {
        public NoteInputType()
        {
            this.Name = "NoteInput";
            this.Description = "InputType for adding a new note.";

            Field<NonNullGraphType<StringGraphType>>(
                name: "title",
                description: "The title of the new note.");

            Field<NonNullGraphType<StringGraphType>>(
                name: "content",
                description: "The content of the new note.");
        }
    }
}
