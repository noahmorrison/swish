using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace Swish
{
    class Compiler
    {
        public static void Main(string[] args)
        {
            var ns = new Namespace();
            ns.Add("/usr/lib/mono/4.5/mscorlib.dll");
            ns.Using("System");
            ns.Alias("System.Console.WriteLine", "Print");
            ns.Alias("System.Console.ReadLine", "ReadLine");

            var program = new Program(ns);

            foreach (var arg in args)
                program.Parse(File.ReadAllText(arg));

            program.Emit();
        }
    }
}
