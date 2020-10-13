using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class Registry<TKey,TValue> : SerializedScriptableObject
    {
        #region Encapsuled Types

        [HideReferenceObjectPicker]
        private class KeyValuePair
        {
            public TKey Key => key;
            public TValue Value => value;
            
            [SerializeField] private TKey key;
            [SerializeField] private TValue value;
        }
        #endregion

        public TValue this[TKey key] => values[key];
        
        [SerializeField] private KeyValuePair[] pairs = new KeyValuePair[0];

        private Dictionary<TKey,TValue> values = new Dictionary<TKey,TValue>();

        public bool HasKey(TKey key) => values.ContainsKey(key);
        
        void OnEnable()
        {
            values.Clear();
            foreach (var pair in pairs)
            {
                values.Add(pair.Key, pair.Value);
            }
        }
    }
}