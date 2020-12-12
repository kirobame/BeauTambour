using System;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(3922097781858013310)]
    public abstract class SequenceElement : ScriptableObject
    {
        public abstract bool Contains(Enum key);
    }
    public abstract class SequenceElement<TEnum> : SequenceElement where TEnum : Enum
    {
        [SerializeField] private TEnum[] keys;
        public override bool Contains(Enum key) => keys.Contains((TEnum)key);
    }
}