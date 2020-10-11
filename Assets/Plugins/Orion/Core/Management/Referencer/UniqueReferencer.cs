using UnityEngine;

namespace Orion
{
    public class UniqueReferencer : ReferencerBase
    {
        public override object Content => value;
        public Object Value => value;
        
        [SerializeField] private Object value;

        protected override void Register() => Repository.Register(value);
        protected override void Unregister() => Repository.Unregister(value.GetType());
    }
}