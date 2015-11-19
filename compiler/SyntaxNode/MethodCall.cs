using System;
using System.Collections.Generic;
using System.Linq;

namespace Swish
{
    class MethodCall : ISyntaxNode, IExpression
    {
        public Identifier Name { get; private set; }
        public IExpression[] Args { get; private set; }
        public IType ReturnType { get; private set; }

        public MethodCall(Identifier name, params IExpression[] args)
        {
            Name = name;
            Args = args;

            foreach (var arg in args)
                if (arg.ReturnType == new AsmType("System.Void"))
                    throw new ArgumentException();

            ReturnType = new AsmType("System.Void");
        }

        public MethodCall(Identifier name, IType returnType, params IExpression[] args)
            : this(name, args)
        {
            ReturnType = returnType;
        }

        public void Emit(Emitter e)
        {
            foreach (var arg in Args)
                arg.Emit(e);

            e.EmitLine("call " + ReturnType.Name + " " + Name.Value);
        }

        public static MethodCall Parse(ref string input, Namespace ns)
        {
            var orgInput = input;

            var ident = Identifier.Parse(ref input);
            if (ident == null)
                return null;

            if (input[0] != '(')
                return null;
            input = input.Substring(1);

            var length = input.Length;
            var args = new List<IExpression>();
            while (true)
            {
                var expr = Expression.Parse(ref input, ns);
                if (expr != null)
                {
                    if (expr.ReturnType.Name == "System.Void")
                        throw new ArgumentException("Argument must have a return type");

                    args.Add(expr);
                }

                Whitespace.Parse(ref input);
                if (input.Length == 0)
                {
                    input = orgInput;
                    return null;
                }

                if (input[0] == ',')
                {
                    input = input.Substring(1);
                    continue;
                }

                else if (input[0] == ')')
                {
                    input = input.Substring(1);
                    break;
                }

                if (length == input.Length)
                    throw new Exception("Could not parse:" + Environment.NewLine + input);
                length = input.Length;
            }

            var method = ns.GetMethod(ident.Value, args.Select(e => e.ReturnType).ToArray());
            if (method == null)
                throw new Exception("Could not find method: " + ident.Value);

            var returnType = new AsmType(method.Split(' ')[0]);
            var methodName = string.Join(" ", method.Split(' ').Skip(1));

            ident = new Identifier(methodName);
            return new MethodCall(ident, returnType, args.ToArray());
        }
    }
}
