namespace Swish
{
    interface IExpression : ISyntaxNode
    {
        IType ReturnType { get; }
    }

    static class Expression
    {
        public static IExpression Parse(ref string input, Namespace ns)
        {
            var orgInput = input;

            var literal = Literal.Parse(ref input);
            if (literal != null)
                return literal;

            input = orgInput;
            var method = MethodCall.Parse(ref input, ns);
            if (method != null)
                return method;

            input = orgInput;
            var assignment = Assignment.Parse(ref input, ns);
            if (assignment != null)
            {
                assignment.ExprMode = true;
                return assignment;
            }

            input = orgInput;
            var variable = Variable.Parse(ref input, ns);
            if (variable != null)
                return variable;

            input = orgInput;
            return null;
        }
    }
}
