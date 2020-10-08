using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Orion
{
    [HideReferenceObjectPicker]
    public class WrappedValue<T> : IProxy<T>
    {
        [SerializeField] private T value;
        
        object IReadable.Read() => Read();
        public T Read() => value;

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value) => this.value = value;
    }
}