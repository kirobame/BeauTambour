using System;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public abstract class Phase : MonoBehaviour, IPhase
    {
        event Action IPhase.onEnd
        {
            add => onEnd += value;
            remove => onEnd -= value;
        }
        private event Action onEnd;
        
        public PhaseType Type => type;
        [SerializeField] private PhaseType type;

        void Awake()
        {
            Event.Open($"{type}.{PhaseCallback.Start}");
            Event.Open($"{type}.{PhaseCallback.End}");
        }

        public virtual void Begin() => Event.Call($"{type}.{PhaseCallback.Start}");
        protected void End()
        {
            onEnd?.Invoke();
            Event.Call($"{type}.{PhaseCallback.End}");
        }
    }
}