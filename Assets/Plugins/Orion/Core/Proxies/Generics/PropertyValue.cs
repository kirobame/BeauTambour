using System;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    [Inlined, HideReferenceObjectPicker]
    public class PropertyValue<T> : IProxy<T>
    {
        [SerializeField] private Object target;
        [SerializeField, PropertySelector("target", 0)] private string property = "None";

        private bool hasBeenInitialized;
        
        private Action<T> setter;
        private Func<T> getter;
        
        object IReadable.Read() => Read();
        public T Read()
        {
            if (!hasBeenInitialized) BootUp();
            return getter();
        }

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value)
        {
            if (!hasBeenInitialized) BootUp();
            setter(value);
        }

        private void BootUp()
        {
            var info = target.GetType().GetProperty(property, BindingFlags.Instance | BindingFlags.Public);
            
            getter = info.GetMethod.CreateDelegate(typeof(Func<T>)) as Func<T>;
            setter = info.SetMethod.CreateDelegate(typeof(Action<T>)) as Action<T>;

            hasBeenInitialized = true;
        }
    }
}