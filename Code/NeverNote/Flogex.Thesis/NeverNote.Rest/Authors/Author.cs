using System;
using System.Collections.Generic;
using Flogex.Thesis.NeverNote.Rest.Notes;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Models.Annotation;

namespace Flogex.Thesis.NeverNote.Rest.Authors
{
    [Resource("authors")]
    public class Author : Identifiable<int>
    {
        public Author(int id, string userName, string givenName, string familyName, string email,
            DateTime? birthDate)
        {
            this.Id = id;
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.GivenName = givenName ?? throw new ArgumentNullException(nameof(givenName));
            this.FamilyName = familyName ?? throw new ArgumentNullException(nameof(familyName));
            this.Email = email ?? throw new ArgumentNullException(nameof(email));
            this.BirthDate = birthDate;
        }

        public override int Id { get; set; }

        [Attr]
        public string UserName { get; }

        [Attr]
        public string GivenName { get; }

        [Attr]
        public string FamilyName { get; }

        [Attr]
        public string Email { get; }

        [Attr]
        public DateTime? BirthDate { get; }

        [HasMany]
        public IEnumerable<Note> EditedNotes { get; set; }

        public static Author FromAuthor(Shared.Models.Author author)
        {
            return new Author(
                author.Id,
                author.UserName,
                author.GivenName,
                author.FamilyName,
                author.Email,
                author.BirthDate);
        }
    }
}