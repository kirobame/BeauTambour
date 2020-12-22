using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deprecated
{
    public abstract class Collection<T> : ScriptableObject where T: UnityEngine.Object
    {
        public T this[int index] => values[index];
        
        public IReadOnlyList<T> Values => values;
        [SerializeField] protected T[] values;
    }
}