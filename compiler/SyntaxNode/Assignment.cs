namespace Swish
{
    class Assignment : ISyntaxNode, IExpression
    {
        public bool ExprMode { get; set; }
        public IType ReturnType
        {
            get
            {
                if (ExprMode)
                    return Expr.ReturnType;
                return new AsmType("System.Void");
            }
        }

        public Identifier Name { get; private set; }
        public IExpression Expr { get; private set; }
        private int id;

        public Assignment(Identifier name, IExpression expr, int id)
        {
            Name = name;
            Expr = expr;
            this.id = id;
        }

        public void Emit(Emitter e)
        {
            Expr.Emit(e);
            e.EmitLine("stloc " + id);

            if (ExprMode)
                e.EmitLine("ldloc " + id);
        }

        public static Assignment Parse(ref string input, Namespace ns)
        {
            var orgInput = input;

            var ident = Identifier.Parse(ref input);
            if (ident == null)
                return null;

            Whitespace.Parse(ref input);

            if (input[0] != '=')
                return null;
            input = input.Substring(1);

            Whitespace.Parse(ref input);

            var expr = Expression.Parse(ref input, ns);

            if (ident.Value.Contains("."))
            {
                input = orgInput;
                return null;
            }

            var id = ns.GetScope().Add(ident.Value, expr.ReturnType);
            return new Assignment(ident, expr, id);
        }
    }
}
