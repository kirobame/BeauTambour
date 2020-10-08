using System;

namespace Orion
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PropertySelectorAttribute : Attribute
    {
        public PropertySelectorAttribute(string targetName, Type type)
        {
            this.targetName = targetName;
            this.type = type;
        }
        public PropertySelectorAttribute(string targetName, int genericTypeIndex)
        {
            this.targetName = targetName;
            this.genericTypeIndex = genericTypeIndex;
        }

        public readonly string targetName;
        public readonly Type type;
        public readonly int genericTypeIndex = -1;
    }
}