using System;

namespace Flux
{
    public class IconIndicatorAttribute : Attribute
    {
        public IconIndicatorAttribute(long iconId) => this.iconId = iconId;
        
        public long IconId => iconId;
        private long iconId;
    }
}