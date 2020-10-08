using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [HideReferenceObjectPicker, InlineProperty]
    public class TokenProxy<T> : IProxy<T>
    {
        [SerializeField] private Token token;
        
        object IReadable.Read() => Read();
        public T Read() => (T)Repository.Objects[token];

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value) => Repository.Set<T>(token, value);
    }
}