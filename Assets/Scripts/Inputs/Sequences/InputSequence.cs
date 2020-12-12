using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(8714319771344428160)]
    public abstract class InputSequence : ScriptableObject
    {
        protected MonoBehaviour hook;
        public virtual void Initialize(MonoBehaviour hook) => this.hook = hook;
    }
    public abstract class InputSequence<TEnum, TElement> : InputSequence 
        where TEnum : Enum 
        where TElement : SequenceElement<TEnum>
    {
        [SerializeField] private TElement[] elements;
        
        private TEnum history;
        private int advancement;
        
        public void Advance(int groupIndex, TEnum key)
        {
            if (groupIndex == 0 && elements[0].Contains(key))
            {
                advancement = 0;
                history = key;
                
                return;
            }

            if (groupIndex == advancement + 1 && elements[groupIndex].Contains(key))
            {
                advancement = groupIndex;
                history = Combine(history, key);
            }

            if (advancement == elements.Length - 1)
            {
                HandleOutcome(history);

                history = default(TEnum);
                advancement = 0;
            }
        }

        protected abstract TEnum Combine(TEnum history, TEnum key);
        protected abstract void HandleOutcome(TEnum history);
    }
}