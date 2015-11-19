namespace Swish
{
    class Variable : ISyntaxNode, IExpression
    {
        public IType ReturnType { get { return Type; } }
        public readonly string Name;
        public readonly IType Type;
        public readonly int Id;

        public Variable(string name, IType type, int id)
        {
            Name = name;
            Type = type;
            Id = id;
        }

        public void Emit(Emitter e)
        {
            e.EmitLine("ldloc " + Id);
        }

        public static Variable Parse(ref string input, Namespace ns)
        {
            var ident = Identifier.Parse(ref input);
            if (ident == null)
                return null;

            return ns.GetScope().Get(ident.Value);
        }
    }
}
