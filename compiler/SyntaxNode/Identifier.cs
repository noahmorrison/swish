using System;

namespace Swish
{
    class Identifier : ISyntaxNode
    {
        public string Value { get; private set; }

        public Identifier(string val)
        {
            Value = val;
        }

        public void Emit(Emitter e)
        {
            throw new NotImplementedException();
        }

        public static Identifier Parse(ref string input)
        {
            var name = string.Empty;

            if (input.Length == 0 || !char.IsLetter(input[0]))
                return null;

            foreach (var c in input)
                if (char.IsLetter(c) || char.IsDigit(c) || c == '.')
                    name += c;
                else
                    break;

            input = input.Substring(name.Length);
            return new Identifier(name);
        }
    }
}
