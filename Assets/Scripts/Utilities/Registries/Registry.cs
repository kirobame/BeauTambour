using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deprecated;
using UnityEngine;

namespace BeauTambour
{
    public abstract class Registry<TKey,TValue> : ScriptableObject
    {
        public TValue this[TKey key]
        {
            get
            {
                if (!hasBeenBootedUp)
                {
                    BootUp();
                    hasBeenBootedUp = true;
                }
                
                return dictionary[key];
            }
        }

        public IReadOnlyList<KeyValuePair<TKey, TValue>> KeyValuePairs => keyValuePairs;
        
        protected abstract KeyValuePair<TKey, TValue>[] keyValuePairs { get; }
        private Dictionary<TKey, TValue> dictionary;

        private bool hasBeenBootedUp;

        private void BootUp()
        {
            dictionary = new Dictionary<TKey, TValue>();
            foreach (var keyValuePair in keyValuePairs)
            {
                if (dictionary.ContainsKey(keyValuePair.Key)) continue;
                
                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
        
        public bool TryGet(TKey key, out TValue value)
        {
            if (!hasBeenBootedUp)
            {
                BootUp();
                hasBeenBootedUp = true;
            }
            
            if (dictionary.TryGetValue(key, out value)) return true;
            else return false;
        }
    }
}