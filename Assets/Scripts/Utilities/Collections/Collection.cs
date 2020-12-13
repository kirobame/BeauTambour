using System.Collections;
using UnityEngine;

namespace BeauTambour
{
    public abstract class Collection<T> : ScriptableObject where T: UnityEngine.Object
    {
        [SerializeField] protected T[] values;
    }
}