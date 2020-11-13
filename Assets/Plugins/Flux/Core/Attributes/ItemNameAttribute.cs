using System;

namespace Flux
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ItemNameAttribute : Attribute
    {
        public ItemNameAttribute(string name) => this.name = name;

        public string Name => name;
        private string name;
    }
}