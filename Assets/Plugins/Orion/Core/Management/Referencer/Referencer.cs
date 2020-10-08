using UnityEngine;

namespace Orion
{
    public class Referencer : ReferencerBase
    {
        public override object Content => value;
        public Object Value => value;
        
        [SerializeField] private Object value;
        
        protected override void Register() => Repository.Register(this);
        protected override void Unregister() => Repository.Unregister(this);

        public void SetValue(object value) => this.value = (Object)value;
    }
}