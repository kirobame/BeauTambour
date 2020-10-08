using System;
using System.Reflection;

namespace Orion
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MethodSelectorAttribute : Attribute
    {
        public MethodSelectorAttribute(string targetName, Type returnType, bool allowGenerics)
        {
            this.targetName = targetName;
            this.returnType = returnType;
            this.allowGenerics = allowGenerics;
        }

        public readonly string targetName;
        public readonly Type returnType;
        public readonly bool allowGenerics;
    }
}