using System;
using UnityEngine;

namespace Deprecated
{
    [Serializable]
    public class KeyValuePair { }
    
    [Serializable]
    public class KeyValuePair<TKey, TValue> : KeyValuePair
    {
        public TKey Key => key;
        public TValue Value => value;
        
        [SerializeField] private TKey key;
        [SerializeField] private TValue value;
    }
}