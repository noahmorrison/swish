namespace Swish
{
    class StringLiteral : ISyntaxNode, IExpression, ILiteral
    {
        public string Value { get; private set; }
        public IType ReturnType
        {
            get { return new AsmType("System.String"); }
        }

        public StringLiteral(string value)
        {
            Value = value;
        }

        public void Emit(Emitter e)
        {
            e.EmitLine("ldstr \"" + Value + "\"");
        }

        public static StringLiteral Parse(ref string input)
        {
            var literal = string.Empty;
            if (input[0] != '"')
                return null;

            foreach (var c in input.Substring(1))
                if (c == '"')
                    break;
                else
                    literal += c;

            input = input.Substring(literal.Length + 2);
            return new StringLiteral(literal);
        }
    }
}
