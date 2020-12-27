using System;
using UnityEngine;

namespace BeauTambour
{
    [Serializable]
    public class KeyValuePair { }

    [Serializable]
    public class KeyValuePair<TKey, TValue> : KeyValuePair
    {
        public KeyValuePair() { }
        public KeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
        
        public TKey Key => key;
        public TValue Value => value;
        
        [SerializeField] private TKey key;
        [SerializeField] private TValue value;
    }
}