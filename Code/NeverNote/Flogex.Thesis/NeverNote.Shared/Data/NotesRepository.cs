using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.Shared.Data
{
    public class NotesRepository
    {
        private static int _idCounter = 1;

        private Dictionary<int, Note> _notes = new Dictionary<int, Note>();

        public async ValueTask Initialize(AuthorsRepository authors)
        {
            var author1 = (await authors[1])!.Value;
            var author2 = (await authors[2])!.Value;
            var author3 = (await authors[3])!.Value;
            var author4 = (await authors[4])!.Value;

            var notes = new Note[]
            {
                new Note(_idCounter++, "Einkaufsliste:", "- Milch\r\n- 10 Eier\r\n- Mehl 2x\r\n- Wildlachsfilet\r\n- Kartoffeln, festkochend, 1kg\r\n- Wasser naturell",
                    author1, UtcDateTime(2020, 03, 22),
                    new Author[] { author3, author4 },
                    new Keyword[] { "Supermarkt", "Einkaufen", "Essen", "Lecker" }),

                new Note(_idCounter++, "Telefonnummern", "- Birgit: 06304 881710\r\n- Martin: 04661 140372\r\n- Martina: 03946 133192\r\n- Frau Gärtner: 07361 839716\r\n- Herr Fiedler: 03473 556168\r\n- Mandy: 09171 868797\r\n- Stephan: 036453 42651",
                    author4, UtcDateTime(2020, 05, 22),
                    new Author[] { author1 },
                    new Keyword[] { "Kontakte" }),

                new Note(_idCounter++, "Zitate",
                    "\"Life is a banquet. And the tragedy is that most people are starving to death.\" (Anthony de Mello)\r\n" +
                    "\"Every great idea starts out as a blasphemy.\" (Bertrand Russel)",
                    author2, UtcDateTime(2020, 05, 23),
                    new Author[] { author3 },
                    new Keyword[] { "Weisheit", "Infinite Love" }),

                new Note(_idCounter++, "Pi", "3.141592653589793238462643383279502884197169399375105820974945",
                    author3, UtcDateTime(2020, 06, 05),
                    new Author[0],
                    new Keyword[] { "Mathematik", "Konstanten" }),

                new Note(_idCounter++, "Eulersche Zahl", "2.718281828459",
                    author3, UtcDateTime(2020, 06, 10),
                    new Author[0],
                    new Keyword[] { "Mathematik", "Konstanten" }),

                new Note(_idCounter++, "Eulersche Identität", "e ^ (i⋅π) + 1 = 0",
                    author3, UtcDateTime(2020, 07, 01),
                    new Author[] { author2, author4 },
                    new Keyword[] { "Mathematik", "Formeln", "Schönheit" }),

                new Note(_idCounter++, "Kosinussatz", "c^2 = a^2 + b^2 – 2ab * cos γ",
                    author2, UtcDateTime(2020, 07, 01),
                    new Author[] { author4 },
                    new Keyword[] { "Mathematik", "Formeln" }),

                new Note(_idCounter++, "Ausbreitungsgeschwindigkeit elektromagnetischer Wellen im Vakuum", "c = 1 / √(ε0 ·μ0)",
                    author2, UtcDateTime(2020, 07, 03),
                    new Author[0],
                    new Keyword[] { "Physik", "Formeln" }),

                new Note(_idCounter++, "Schönes Gedicht", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                    author4, UtcDateTime(2020, 08, 31),
                    new Author[] { author3 },
                    new Keyword[] { "Lorem", "Literatur", "#Latein muss sein" }),

                new Note(_idCounter++, "Leseliste", "• Stephen Covey: 7 Habits of Highly Effective People\r\n• John Yates: The Illuminated Mind\r\n• Stefan Baron: Die Chinesen\r\n•Harper Lee: Wer die Nachtigall stört\r\n• John C. Maxwell: Failing Forward\r\n• Greg McKeown: Essentialism: The Disciplined Pursuit of Less\r\n• Yuval Noah Harari: 21 Lektionen für das 21.Jahrhundert\r\n• Friedrich Dürrenmatt: Die Physiker\r\n• Sandi Metz: 99 Bottles of OOP",
                   author1, UtcDateTime(2020, 08, 31),
                    new Author[0],
                    new Keyword[] { "Bücher", "Lesen", "Leseratte", "Wissen", "Weiterbildung" })
            };

            _notes = notes.ToDictionary(note => note.Id);
        }

        private static DateTime UtcDateTime(int year, int month, int day)
            => new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);

        public ValueTask<IEnumerable<Note>> GetAll(CancellationToken cancellationToken = default)
            => new ValueTask<IEnumerable<Note>>(_notes.Values.AsEnumerable());

        public ValueTask<IEnumerable<Note>> GetByTitle(string titlePrefix, CancellationToken cancellationToken = default)
            => new ValueTask<IEnumerable<Note>>(_notes.Values.Where(note => note.Title.StartsWith(titlePrefix)));

        public ValueTask<IEnumerable<Note>> GetByAuthor(Author author, CancellationToken cancellationToken = default)
            => new ValueTask<IEnumerable<Note>>(_notes.Values.Where(note => note.Creator == author || note.Contributors.Contains(author)));

        public ValueTask<IEnumerable<Note>> GetByKeyword(string keyword, CancellationToken cancellationToken = default)
            => new ValueTask<IEnumerable<Note>>(_notes.Values.Where(note => note.Keywords.Contains(keyword)));

        public ValueTask<Note?> GetById(int id, CancellationToken cancellationToken = default)
            => _notes.TryGetValue(id, out var note)
            ? new ValueTask<Note?>(note)
            : new ValueTask<Note?>();

        public ValueTask<Note?> this[int id]
            => GetById(id);

        public ValueTask<Note> AddNote(
            NoteInput input,
            Author creator,
            CancellationToken cancellationToken = default)
        {
            var newNote = new Note(
                _idCounter++,
                input.Title,
                input.Content,
                creator,
                DateTime.UtcNow,
                new List<Author>(),
                new List<Keyword>());

            _notes.Add(newNote.Id, newNote);
            return new ValueTask<Note>(newNote);
        }

        public ValueTask<bool> UpdateNote(
            int id,
            NoteInput input,
            Author editor,
            CancellationToken cancellationToken = default)
        {
            var exists = _notes.TryGetValue(id, out var oldNote);

            if (!exists)
                return new ValueTask<bool>(false);

            var mustAddContributor = oldNote.Creator != editor &&
                !oldNote.Contributors.Contains(editor);

            var updatedNote = new Note(
                oldNote.Id,
                input.Title ?? oldNote.Title,
                input.Content ?? oldNote.Content,
                oldNote.Creator,
                oldNote.DateCreated,
                mustAddContributor
                    ? oldNote.Contributors.Append(editor).ToList()
                    : oldNote.Contributors.ToList(),
                oldNote.Keywords.ToList());

            _notes.Remove(id);
            _notes.Add(id, updatedNote);

            return new ValueTask<bool>(true);
        }

        public ValueTask<bool> AddKeywordToNote(
            int id,
            string keyword,
            Author editor,
            CancellationToken cancellationToken = default)
        {
            var exists = _notes.TryGetValue(id, out var oldNote);

            if (!exists)
                return new ValueTask<bool>(false);

            var mustAddContributor = oldNote.Creator != editor &&
                !oldNote.Contributors.Contains(editor);

            var updatedNote = new Note(
                oldNote.Id,
                oldNote.Title,
                oldNote.Content,
                oldNote.Creator,
                oldNote.DateCreated,
                mustAddContributor
                    ? oldNote.Contributors.Append(editor).ToList()
                    : oldNote.Contributors.ToList(),
                oldNote.Keywords.Append(keyword).Distinct().ToList());

            // Do not change _notes in order to enable concurrent JMeter tests
            //_notes.Remove(id);
            //_notes.Add(id, updatedNote);

            return new ValueTask<bool>(true);
        }

        // Do not call _notes.Remove(id) in order to enable concurrent JMeter tests
        public ValueTask<bool> Remove(int id, CancellationToken cancellationToken = default)
            => new ValueTask<bool>(_notes.ContainsKey(id));

        public readonly struct NoteInput : IEquatable<NoteInput>
        {
            public NoteInput(string title, string content)
            {
                this.Title = title ?? throw new ArgumentNullException(nameof(title));
                this.Content = content ?? string.Empty;
            }

            public string Title { get; }

            public string Content { get; }

            public bool Equals(NoteInput other)
                => this.Title.Equals(other.Title) && this.Content.Equals(other.Content);

            public override bool Equals(object? obj)
                => obj is NoteInput noteInput && Equals(noteInput);

            public override int GetHashCode()
                => HashCode.Combine(this.Title, this.Content);

            public static bool operator ==(NoteInput left, NoteInput right)
                => left.Equals(right);

            public static bool operator !=(NoteInput left, NoteInput right)
                => !left.Equals(right);
        }
    }
}
