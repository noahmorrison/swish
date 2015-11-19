using System.Collections.Generic;
using System.Linq;

namespace Swish
{
    interface IMethod
    {
        VariableScope Variables { get; set; }
        IType ReturnType { get; }
        string Name { get; set; }
    }

    class Method: ISyntaxNode, IMethod
    {
        public string Name { get; set; }
        public VariableScope Variables { get; set;}
        public IType ReturnType
        {
            get
            {
                if (!Body.Any())
                    return new AsmType("System.Void");

                var expr = Body.Last() as IExpression;
                if (expr == null)
                    return new AsmType("System.Void");

                return expr.ReturnType;
            }
        }

        public List<ISyntaxNode> Body;

        public Method(string name, params ISyntaxNode[] body)
        {
            Name = name;
            Body = new List<ISyntaxNode>(body);
        }

        public void Emit(Emitter e)
        {
            e.EmitLine(".method static void " + Name + "()");
            e.Indent();
            Variables.Emit(e);

            foreach (var item in Body)
                item.Emit(e);

            e.EmitLine("ret");
            e.Outdent();
        }
    }
}
