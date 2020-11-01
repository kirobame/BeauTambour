using System;

namespace Flux
{
    public class IconProxyAttribute : Attribute
    {
        public IconProxyAttribute(Type target) => this.target = target;
        
        public Type Target => target;
        private Type target;
    }
}