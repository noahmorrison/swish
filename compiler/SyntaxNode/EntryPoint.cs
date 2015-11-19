using System.Collections.Generic;


namespace Swish
{
    class EntryPoint : ISyntaxNode, IMethod
    {
        public string Name { get; set; }
        public IType ReturnType
        {
            get { return new AsmType("System.Void"); }
            set { }
        }
        public Namespace Namespace { get; set; }
        public VariableScope Variables { get; set; }
        public List<ISyntaxNode> Body { get; set; }

        public EntryPoint(Namespace ns, params ISyntaxNode[] body)
        {
            Name = "~Main";
            Body = new List<ISyntaxNode>(body);
            Variables = ns.AddScope();
            Namespace = ns;
        }

        public void Emit(Emitter e)
        {
            e.EmitLine(".method static void " + Name + "()");
            e.Indent();
            e.EmitLine(".entrypoint");
            Variables.Emit(e);

            foreach (var item in Body)
                item.Emit(e);

            e.EmitLine("ret");
            e.Outdent();
        }
    }
}
