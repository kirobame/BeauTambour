using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class Stash<T> : SerializedScriptableObject
    {
        public IReadOnlyList<T> Values => values;
        [SerializeField] private T[] values = new T[0];
    }
}