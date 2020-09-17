using System;

namespace Flogex.Thesis.NeverNote.Shared.Models
{
    public readonly struct Author : IEquatable<Author>
    {
        public Author(
            int id,
            string userName,
            string givenName,
            string familyName,
            string email,
            DateTime? birthDate)
        {
            this.Id = id;
            this.UserName = userName;
            this.GivenName = givenName;
            this.FamilyName = familyName;
            this.Email = email;
            this.BirthDate = birthDate;
        }

        public int Id { get; }

        public string UserName { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Email { get; }

        public DateTime? BirthDate { get; }

        public override bool Equals(object? obj)
            => obj is Author author && Equals(author);

        public bool Equals(Author other) => this.Id == other.Id;

        public override int GetHashCode()
            => HashCode.Combine(this.Id, this.GivenName, this.FamilyName, this.Email, this.BirthDate);

        public static bool operator ==(Author left, Author right) => left.Equals(right);

        public static bool operator !=(Author left, Author right) => !(left == right);
    }
}
