using System;
using System.Reflection;
using System.Linq;

namespace Swish
{
    enum TypeModifier
    {
        Normal,
        Reference,
    }

    interface IType
    {
        TypeModifier Modifier { get; set; }
        string Name { get; }
        string GetMethod(string methodName, params IType[] types);
    }
}
