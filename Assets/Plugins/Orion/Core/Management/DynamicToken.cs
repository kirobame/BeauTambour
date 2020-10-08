using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    /// <summary>
    /// A single asset key which allows access to a collection of instance by providing a secondary <code>Object</code> key.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDynamicToken", menuName = "Orion/Tokens/Dynamic")]
    public class DynamicToken : ScriptableObject
    {
        public IReadOnlyDictionary<int, object> Registry => registry;
        private Dictionary<int, object> registry = new Dictionary<int, object>();

        /// <summary>
        /// Register a value at a uniqye <code>Object</code> key.
        /// </summary>
        /// <param name="owner">The key.</param>
        /// <param name="value">The value.</param>
        public void Register(Object owner, object value)
        {
            try
            {
                registry.Add(owner.GetInstanceID(), value);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                return;
            }
        }
        /// <summary>
        /// Removes the reference at the <code>Object</code> key.
        /// </summary>
        /// <param name="owner">The key.</param>
        /// <returns>Indicates if the removal was successful.</returns>
        public bool Unregister(Object owner) => registry.Remove(owner.GetInstanceID());

        /// <summary>
        /// Gets the reference at the <code>Object</code> key and cast it as a <code>T</code> instance.
        /// </summary>
        /// <param name="owner">The key.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        /// <returns>The casted instance.</returns>
        public T Get<T>(Object owner) => (T)registry[owner.GetInstanceID()];
        /// <summary>
        /// Tries to get the reference at the <code>Object</code> key and cast it as a <code>T</code> instance.
        /// </summary>
        /// <param name="owner">The key.</param>
        /// <param name="value">The outgoing casted instance.</param>
        /// <typeparam name="T">the cast type.</typeparam>
        /// <returns>Indicates if the operation was successful.</returns>
        public bool TryGet<T>(Object owner, out T value)
        {
            if (registry.TryGetValue(owner.GetInstanceID(), out var rawValue))
            {
                value = (T)rawValue;
                return true;
            }
            
            value = default;
            return false;
        }
        
        /// <summary>
        /// Sets the reference value at the <code>Object</code> key
        /// </summary>
        /// <param name="owner">The key.</param>
        /// <param name="value">The new value to be assigned.</param>
        public void Set<T>(Object owner, T value)
        {
            var id = owner.GetInstanceID();
            
            if (registry.ContainsKey(id)) registry[id] = value;
            else Register(owner, value);
        }
    }
}