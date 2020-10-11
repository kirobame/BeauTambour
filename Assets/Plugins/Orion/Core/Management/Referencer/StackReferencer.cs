using System.Collections.Generic;
using UnityEngine;

namespace Orion
{
    public class StackReferencer : ReferencerBase
    {
        public Token Token => token;
        
        public override object Content => values;
        public IReadOnlyList<Object> Values => values;

        [SerializeField] private Token token;
        [SerializeField] private Object[] values;
        
        protected override void Register() => Repository.RegisterStack(this);
        protected override void Unregister() => Repository.RemoveFromStack(this);

        public void SetValue(object value, int index) => values[index] = (Object)value;
    }
}