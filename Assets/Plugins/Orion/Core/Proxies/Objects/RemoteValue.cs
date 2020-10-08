using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class RemoteValue<T> : SerializedScriptableObject, IProxy<T>
    {
        [SerializeField, HideReferenceObjectPicker] protected T value;
        
        object IReadable.Read() => Read();
        public virtual T Read() => value;

        void IWritable.Write(object value) => Write((T)value);
        public virtual void Write(T value) => this.value = value;
    }
}