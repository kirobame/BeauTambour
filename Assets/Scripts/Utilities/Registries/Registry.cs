using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    public abstract class Registry<TKey,TValue> : ScriptableObject
    {
        public TValue this[TKey key] => dictionary[key];
        
        protected abstract KeyValuePair<TKey, TValue>[] keyValuePairs { get; }
        private Dictionary<TKey, TValue> dictionary;
        
        void OnEnable()
        {
            dictionary = new Dictionary<TKey, TValue>();
            foreach (var keyValuePair in keyValuePairs)
            {
                if (dictionary.ContainsKey(keyValuePair.Key)) continue;
                
                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public TValue Get(TKey key) => keyValuePairs.First(keyValuePair => keyValuePair.Key.Equals(key)).Value;
    }
}