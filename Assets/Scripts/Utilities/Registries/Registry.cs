using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    public abstract class Registry<TKey,TValue> : ScriptableObject, IBootable
    {
        public TValue this[TKey key] => dictionary[key];
        
        protected abstract KeyValuePair<TKey, TValue>[] keyValuePairs { get; }
        private Dictionary<TKey, TValue> dictionary;

        public void BootUp()
        {
            dictionary = new Dictionary<TKey, TValue>();
            foreach (var keyValuePair in keyValuePairs)
            {
                if (dictionary.ContainsKey(keyValuePair.Key)) continue;
                
                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public TValue Get(TKey key) => keyValuePairs.First(keyValuePair => keyValuePair.Key.Equals(key)).Value;
        public bool TryGet(TKey key, out TValue value)
        {
            if (dictionary.TryGetValue(key, out value)) return true;
            else return false;
        }

        public TValue GetSafe(TKey key)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                if (keyValuePair.Key.Equals(key)) return keyValuePair.Value;
            }

            return default;
        }
    }
}