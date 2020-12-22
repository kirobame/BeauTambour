using System;
using Flux;
using UnityEngine;

namespace Deprecated
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
        [SerializeField] protected TElement[] elements;
        
        private TEnum history;
        private int advancement = -1;

        void OnEnable() => advancement = -1;

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
            else HandleFailure(history, groupIndex, key);

            if (advancement == elements.Length - 1)
            {
                HandleOutcome(history);

                history = default(TEnum);
                advancement = -1;
            }
        }

        protected abstract TEnum Combine(TEnum history, TEnum key);
        protected abstract void HandleOutcome(TEnum history);
        protected abstract void HandleFailure(TEnum history, int groupIndex, TEnum key);
    }
}