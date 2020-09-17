using System;

namespace Flogex.Thesis.NeverNote.Shared.Models
{
    public readonly struct Keyword : IEquatable<Keyword>
    {
        public Keyword(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public override bool Equals(object? obj)
            => obj is Keyword keyword && Equals(keyword);

        public bool Equals(Keyword other)
            => this.Value == other.Value;

        public override int GetHashCode()
            => HashCode.Combine(this.Value);

        public static implicit operator Keyword(string str) => new Keyword(str);

        public static implicit operator string(Keyword keyword) => keyword.Value;
    }
}
