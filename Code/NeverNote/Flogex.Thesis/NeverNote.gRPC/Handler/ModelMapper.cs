using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Models = Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.gRPC.Handler
{
    public static class ModelMapper
    {
        public static Note MapNote(Models.Note note)
        {
            var mapped = new Note
            {
                Id = new Note.Types.Id { Value = (uint)note.Id },
                Title = note.Title,
                Content = note.Content,
                Creator = GetAuthorId(note.Creator),
                DateCreated = Timestamp.FromDateTime(note.DateCreated)
            };

            mapped.Contributors.Add(note.Contributors.Select(GetAuthorId));
            mapped.Keywords.Add(note.Keywords.Select(_ => _.Value));

            return mapped;
        }

        public static Author MapAuthor(Models.Author author)
            => new Author
            {
                Id = GetAuthorId(author),
                UserName = author.UserName,
                GivenName = author.GivenName,
                FamilyName = author.FamilyName,
                Email = author.Email,
                Birthdate = author.BirthDate.HasValue
                    ? Timestamp.FromDateTime(author.BirthDate.Value)
                    : null
            };

        public static Author.Types.Id GetAuthorId(Models.Author author)
            => new Author.Types.Id { Value = (uint)author.Id };
    }
}
