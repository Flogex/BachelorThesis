using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.Shared.Data
{
    public class AuthorsRepository
    {
        private static int _idCounter = 1;

        private Dictionary<int, Author> _authors = new Dictionary<int, Author>();

        public ValueTask Initialize()
        {
            var authors = new Author[]
            {
                new Author(
                    id: _idCounter++,
                    userName: "bkoehler84",
                    givenName: "Birgit",
                    familyName: "Köhler",
                    email: "BirgitKoehler@cuvox.de",
                    birthDate: UtcDateTime(1984, 1, 2)),
                new Author(
                    id: _idCounter++,
                    userName: "TheGoldenLeon",
                    givenName: "Leon",
                    familyName: "Leon Goldschmidt",
                    email: "LGoldschmidt@einrot.com",
                    birthDate: UtcDateTime(1954, 6, 20)),
                new Author(
                    id: _idCounter++,
                    userName: "anauma",
                    givenName: "Anne",
                    familyName: "Naumann",
                    email: "naumann@einrot.com",
                    birthDate: UtcDateTime(1986, 7, 9)),
                new Author(
                    id: _idCounter++,
                    userName: "jensi",
                    givenName: "Jens",
                    familyName: "Finkel",
                    email: "JensFinkel@cuvox.de",
                    birthDate: null),
                new Author(
                    id: _idCounter++,
                    userName: "flogex",
                    givenName: "Florian",
                    familyName: "Gerlinghoff",
                    email: "florian.gerlinghoff@stud.htwk-leipzig.de",
                    birthDate: null)
            };

            _authors = authors.ToDictionary(author => author.Id);

            return new ValueTask();
        }

        private static DateTime UtcDateTime(int year, int month, int day)
            => new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);

        public ValueTask<IEnumerable<Author>> GetAll(CancellationToken cancellationToken = default)
            => new ValueTask<IEnumerable<Author>>(_authors.Values.AsEnumerable());

        public ValueTask<Author?> GetByUserName(string userName, CancellationToken cancellationToken = default)
        {
            var result = _authors.Values.SingleOrDefault(a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            return result != default
                ? new ValueTask<Author?>(result)
                : new ValueTask<Author?>();
        }

        public ValueTask<Author?> GetById(int id, CancellationToken cancellationToken = default)
            => _authors.TryGetValue(id, out var result)
            ? new ValueTask<Author?>(result)
            : new ValueTask<Author?>();

        public ValueTask<Author?> this[int id]
            => GetById(id);

        public ValueTask<Author> AddAuthor(
            AuthorInput input,
            CancellationToken cancellationToken = default)
        {
            var newAuthor = new Author(
                id: _idCounter++,
                userName: input.UserName,
                givenName: input.GivenName,
                familyName: input.FamilyName,
                email: input.Email,
                birthDate: input.BirthDate?.ToUniversalTime());

            _authors.Add(newAuthor.Id, newAuthor);
            return new ValueTask<Author>(newAuthor);
        }

        public readonly struct AuthorInput : IEquatable<AuthorInput>
        {
            // GraphQL cannot use constructors with default arguments
            public AuthorInput(string userName, string givenName, string familyName, string email)
                : this(userName, givenName, familyName, email, null) { }

            public AuthorInput(string userName, string givenName, string familyName, string email, DateTime? birthDate)
            {
                this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
                this.GivenName = givenName ?? throw new ArgumentNullException(nameof(givenName));
                this.FamilyName = familyName ?? throw new ArgumentNullException(nameof(familyName));
                this.Email = email ?? throw new ArgumentNullException(nameof(email));
                this.BirthDate = birthDate;
            }

            public string UserName { get; }

            public string GivenName { get; }

            public string FamilyName { get; }

            public string Email { get; }

            public DateTime? BirthDate { get; }

            public bool Equals(AuthorInput other)
                => this.UserName == other.UserName && this.Email == other.Email;

            public override bool Equals(object? obj)
                => obj is AuthorInput input && Equals(input);

            public override int GetHashCode()
                => HashCode.Combine(this.UserName, this.Email, this.BirthDate);

            public static bool operator ==(AuthorInput left, AuthorInput right)
                => left.Equals(right);

            public static bool operator !=(AuthorInput left, AuthorInput right)
                => !left.Equals(right);
        }
    }
}
