using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Flogex.Thesis.NeverNote.Shared.Models
{
    public readonly struct Note
    {
        public Note(int id, string title, string content, Author creator, DateTime dateCreated,
            IList<Author> contributors, IList<Keyword> keywords)
        {
            this.Id = id;
            this.Title = title;
            this.Content = content;
            this.Creator = creator;
            this.DateCreated = dateCreated;
            this.LastModified = DateTime.UtcNow;
            this.Contributors = new ReadOnlyCollection<Author>(contributors);
            this.Keywords = new ReadOnlyCollection<Keyword>(keywords);
        }

        public int Id { get; }

        public string Title { get; }

        public string Content { get; }

        public Author Creator { get; }

        public DateTime DateCreated { get; }

        public DateTime LastModified { get; }

        public IReadOnlyCollection<Author> Contributors { get; }

        public IReadOnlyCollection<Keyword> Keywords { get; }
    }
}
