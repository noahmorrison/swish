using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Swish
{
    class Namespace
    {
        private List<string> usings;
        private Dictionary<string, string> aliases;
        private Stack<VariableScope> variables;
        private List<IType> types;

        public Namespace()
        {
            usings = new List<string>(new[] { string.Empty });
            aliases = new Dictionary<string, string>();
            variables = new Stack<VariableScope>();
            types = new List<IType>();
        }

        public void Add(string path)
        {
            var assembly = Assembly.LoadFile(path);
            foreach (var type in assembly.GetTypes())
                types.Add(new AsmType(type));
        }

        public VariableScope AddScope()
        {
            var scope = new VariableScope();
            variables.Push(scope);
            return scope;
        }

        public VariableScope GetScope()
        {
            return variables.Peek();
        }

        public string GetMethod(string name, params IType[] args)
        {
            if (aliases.ContainsKey(name))
                name = aliases[name];

            var items = name.Split('.');
            var typeName = string.Join(".", items.Take(items.Length - 1));
            var method = items.Last();

            foreach (var u in usings) try
            {
                var type = new AsmType(u + typeName);
                return type.GetMethod(method, args);
            }
            catch { }

            throw new Exception(string.Format("Type not found: {0}",
                        name, string.Join(", ", args.Select(a => a.Name))));
        }

        public void Using(string name)
        {
            usings.Add(name + ".");
        }

        public void Alias(string fullname, string shortname)
        {
            aliases.Add(shortname, fullname);
        }
    }
}
