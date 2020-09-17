using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using Flogex.Thesis.NeverNote.Shared.Models;

namespace Flogex.Thesis.NeverNote.GraphQL.Types
{
    public class KeywordGraphType : ScalarGraphType
    {
        static KeywordGraphType()
        {
            ValueConverter.Register(typeof(string), typeof(Keyword), ConvertToKeyword);
        }

        private static object? ConvertToKeyword(object input)
        {
            if (input is string str)
                return new Keyword(str);
            else if (input?.ToString() is string stringRepresentation)
                return new Keyword(stringRepresentation);

            return null;
        }

        public KeywordGraphType()
        {
            this.Name = "Keyword";
        }

        // Argument to keyword
        public override object? ParseLiteral(IValue value)
        {
            if (value is KeywordValue keyword)
                return ParseValue(keyword.Value);
            else if (value is StringValue stringValue)
                return ParseValue(stringValue.Value);

            return null;
        }

        // Variable to keyword
        public override object ParseValue(object value)
            => ValueConverter.ConvertTo<Keyword>(value);

        // Keyword to string
        public override object Serialize(object value)
        {
            if (value is Keyword keyword)
                return ValueConverter.ConvertTo<string>(keyword.Value);

            return ValueConverter.ConvertTo<Keyword>(value);
        }
    }

    public class KeywordValue : ValueNode<Keyword>
    {
        public KeywordValue(Keyword value)
        {
            base.Value = value;
        }

        protected override bool Equals(ValueNode<Keyword> node)
            => base.Value.Equals(node.Value);
    }

    public class KeywordAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
            => new KeywordValue((Keyword)value);

        public bool Matches(object value, IGraphType type)
            => value is Keyword;
    }
}
