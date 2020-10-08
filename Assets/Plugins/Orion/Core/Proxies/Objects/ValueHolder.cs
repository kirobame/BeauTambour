using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class ValueHolder<T> : SerializedMonoBehaviour, IProxy<T>
    {
        [SerializeField] private IProxy<T> proxy;
        
        object IReadable.Read() => Read();
        public T Read() => proxy.Read();

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value) => proxy.Write(value);
    }
}