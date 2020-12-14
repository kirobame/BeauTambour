using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public abstract class AdvanceSequenceOperation<TEnum, TElement, TSequence> : SingleOperation 
        where TEnum : Enum 
        where TElement : SequenceElement<TEnum> 
        where TSequence : InputSequence<TEnum, TElement>
    {
        [SerializeField] private TSequence sequence;

        [Space, SerializeField] private int groupIndex;
        [SerializeField] private TEnum key;

        public override void OnStart(EventArgs inArgs)
        {
            sequence.Advance(groupIndex, key);
            Begin(inArgs);
        }
    }
}