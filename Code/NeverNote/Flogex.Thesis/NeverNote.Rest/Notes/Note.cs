using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Flogex.Thesis.JsonHyperSchema;
using Flogex.Thesis.NeverNote.Rest.Authors;
using Flogex.Thesis.NeverNote.Rest.Keywords;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Models.Annotation;

namespace Flogex.Thesis.NeverNote.Rest.Notes
{
    [Resource("notes")]
    public class Note : Identifiable, IHasMeta
    {
        [Attr]
        public override int Id { get; set; }

        [Attr]
        public string Title { get; set; }

        [Attr]
        public string Content { get; set; }

        [Attr]
        public DateTime DateCreated { get; set; }

        [HasOne]
        public Author Creator { get; set; }

        [HasMany]
        public IEnumerable<Author> Contributors { get; set; }

        [HasMany]
        public IEnumerable<Keyword> Keywords { get; set; }

        public static Note FromNote(Shared.Models.Note note)
        {
            return new Note
            {
                Id = note.Id,
                Title = note.Title ?? string.Empty,
                Content = note.Content ?? string.Empty,
                DateCreated = note.DateCreated,
                Creator = Author.FromAuthor(note.Creator),
                Contributors = note.Contributors?.Select(Author.FromAuthor) ?? Enumerable.Empty<Author>(),
                Keywords = note.Keywords?.Select(Keyword.FromKeyword) ?? Enumerable.Empty<Keyword>()
            };
        }

        public Dictionary<string, object> GetMeta()
        {
            var schema = SchemaGenerator.GenerateFromType<Note>();
            using var ms = new MemoryStream();
            var writer = new Utf8JsonWriter(ms);
            var serializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            SchemaSerializer.Serialize(schema, writer, serializerOptions);

            writer.Flush();
            ms.Position = 0;

            using var sr = new StreamReader(ms);
            var serializedSchema = sr.ReadToEnd();

            return new Dictionary<string, object>
            {
                ["schema"] = serializedSchema
            };
        }
    }
}