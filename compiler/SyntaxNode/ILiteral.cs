namespace Swish
{
    interface ILiteral
    {
    }

    static class Literal
    {
        public static IExpression Parse(ref string input)
        {
            var orgInput = input;

            var stringLiteral = StringLiteral.Parse(ref input);
            if (stringLiteral != null)
                return stringLiteral;

            input = orgInput;
            return null;
        }
    }
}
