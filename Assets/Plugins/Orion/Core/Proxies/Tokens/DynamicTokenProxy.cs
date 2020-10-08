using Ludiq.PeekCore;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    [Inlined, HideReferenceObjectPicker]
    public class DynamicTokenProxy<T> : IProxy<T>
    {
        [SerializeField] private DynamicToken token;
        [SerializeField] private Object owner;
        
        object IReadable.Read() => Read();
        public T Read() => token.Get<T>(owner);

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value) => token.Set<T>(owner, value);
    }
}