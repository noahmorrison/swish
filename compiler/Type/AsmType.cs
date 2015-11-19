using System;
using System.Linq;
using System.Reflection;

namespace Swish
{
    class AsmType : IType
    {
        public TypeModifier Modifier { get; set; }
        public Type Type { get; private set; }
        public string Name
        {
            get
            {
                return "[" + Type.Assembly.GetName().Name + "]" + Type.FullName;
            }
        }

        public AsmType(Type type)
        {
            Type = type;
        }

        public AsmType(string name)
        {
            var asm = null as Assembly;
            if (name.StartsWith("["))
            {
                var asmName = name.Substring(1, name.IndexOf("]") - 1);
                name = name.Substring(name.IndexOf("]") + 1);

                asm = Assembly.Load(asmName);
                Type = asm.GetType(name);
            }
            else
                Type = Type.GetType(name);

            if (Type == null)
                throw new Exception("Invalid type");
        }

        public string GetMethod(string methodName, params IType[] args)
        {
            if (args.Any(a => a as AsmType == null))
                return null;

            var types = args.Select(t => (t as AsmType).Type).ToArray();


            MethodInfo method = null;
            if (args.Length > 0)
            {
                var modifiers = args.Select(arg => arg.Modifier == TypeModifier.Reference).ToArray();
                var mods = new ParameterModifier[] { new ParameterModifier(modifiers.Length) };
                for (var i = 0; i < modifiers.Length; i++)
                    mods[0][i] = modifiers[i];

                method = Type.GetMethod(methodName, types, mods);
            }
            else
                method = Type.GetMethod(methodName);

            if (method == null)
                return null;

            var returnType = new AsmType(method.ReturnType);
            var parameters = method.GetParameters()
                                   .Select(p => new AsmType(p.ParameterType));

            return string.Format("{0} {1}::{2}({3})",
                    returnType.Name, Name, methodName,
                    string.Join(", ", parameters.Select(p => p.Name)));
        }
    }
}
