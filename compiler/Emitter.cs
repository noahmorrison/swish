using System;


namespace Swish
{
    class Emitter
    {
        private int depth;

        public Emitter()
        {
            depth = 0;

            EmitLine(".assembly Test {}");
            EmitLine(".assembly extern mscorlib {}");
        }

        public void Emit(string frag)
        {
            Console.Write(new string(' ', depth * 2));
            Console.Write(frag);
        }

        public void EmitLine(string line)
        {
            Console.Write(new string(' ', depth * 2));
            Console.WriteLine(line);
        }

        public void Indent()
        {
            EmitLine("{");
            depth++;
        }

        public void Outdent()
        {
            depth--;
            EmitLine("}");
        }
    }
}
