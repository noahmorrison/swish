using System;
using System.Collections.Generic;


namespace Swish
{
    class Program
    {
        public List<ISyntaxNode> Root { get; set; }
        public EntryPoint Main { get; set; }
        public Namespace Namespace { get; set; }

        public Program(Namespace ns)
        {
            Root = new List<ISyntaxNode>();
            Main = new EntryPoint(ns);
            Namespace = ns;
        }

        public void Emit()
        {
            Root.Add(Main);

            var e = new Emitter();
            Root.ForEach(node => node.Emit(e));
        }

        public void Parse(string input)
        {
            var length = input.Length;
            while (true)
            {
                var expr = Expression.Parse(ref input, Namespace);
                if (expr != null)
                {
                    Main.Body.Add(expr);

                    if (expr.GetType() == typeof(Assignment))
                    {
                        (expr as Assignment).ExprMode = false;
                        Main.Variables.Add(expr as Assignment);
                    }
                }

                Whitespace.Parse(ref input);

                if (input.Length == 0)
                    break;

                if (length == input.Length)
                    throw new Exception("Could not parse: " + Environment.NewLine + input);
                length = input.Length;
            }
        }
    }
}
