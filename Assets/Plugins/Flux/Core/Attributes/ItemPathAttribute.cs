using System;

namespace Flux
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ItemPathAttribute : Attribute
    {
        public ItemPathAttribute(string path) => this.path = path;

        public string Path => path;
        private string path;
    }
}