using System.Collections.Generic;
using System.Linq;

namespace Swish
{
    class VariableScope
    {
        private List<Variable> variables;
        public VariableScope()
        {
            variables = new List<Variable>();
        }

        public int Add(string name, string type)
        {
            return Add(name, new AsmType(type));
        }

        public int Add(string name, IType type)
        {
            if (Get(name, type) != null)
                return Get(name, type).Id;

            variables.Add(new Variable(name, type, variables.Count));
            return variables.Count - 1;
        }

        public int Add(Assignment assignment)
        {
            return Add(assignment.Name.Value, assignment.Expr.ReturnType);
        }

        public Variable Get(string name, IType type)
        {
            foreach (var variable in variables)
                if (variable.Name == name && variable.Type == type)
                    return variable;

            return null;
        }

        public Variable Get(string name)
        {
            foreach (var variable in Enumerable.Reverse(variables))
                if (variable.Name == name)
                    return variable;

            return null;
        }

        public void Emit(Emitter e)
        {
            e.Emit(".locals init (");
            e.Emit(string.Join(", ", variables.Select(v => v.Type.Name)));
            e.EmitLine(")");
        }
    }
}
