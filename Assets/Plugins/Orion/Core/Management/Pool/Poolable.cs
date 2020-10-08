using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Orion
{
    /// <summary>
    /// A wrapper for a <code>T</code> instance allowing said instance to be stored in a <code>Pool</code>.
    /// </summary>
    /// <typeparam name="T">The type which can be stored.</typeparam>
    public abstract class Poolable<T> : SerializedMonoBehaviour, IProxy<T>
    {
        [FoldoutGroup("Events")] public OrionEvent onReboot = new OrionEvent();

        /// <summary>
        /// The wrapped <code>T</code> instance.
        /// </summary>
        public T Value => value;
        [SerializeField] private T value;
         
        private Pool<T> origin;
        
        protected virtual void OnDisable()
        {
            origin.Stock(this);
            origin = null;
        }

        /// <summary>
        /// Specify the pool to which the <code>T</code> instance will return to when its lifetime is over.
        /// </summary>
        /// <param name="origin">The pool in which the <code>T</code> instance will be stored.</param>
        public void SetOrigin(Pool<T> origin) => this.origin = origin;

        /// <summary>
        /// Prepares the <code>T</code> instance for the beginning of its lifetime.
        /// </summary>
        public void Reboot()
        {
            onReboot.Invoke();
            OnReboot();
        }
        protected virtual void OnReboot() { }
        
        object IReadable.Read() => Read();
        /// <summary>
        /// Sets the wrapped <code>T</code> instance.
        /// </summary>
        public T Read() => value;
        
        void IWritable.Write(object value) => Write((T)value);
        /// <summary>
        /// Gets the wrapped <code>T</code> instance.
        /// </summary>
        public void Write(T value) => this.value = value;

    }
}