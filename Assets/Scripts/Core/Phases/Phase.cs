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

        protected virtual void Awake()
        {
            Event.Open($"{type}.{PhaseCallback.Start}");
            Event.Open($"{type}.{PhaseCallback.End}");
        }

        public virtual void Begin()
        {
            Debug.Log($"Beginning phase : {type}");
            Event.Call($"{type}.{PhaseCallback.Start}");
        }
        public virtual void End()
        {
            Debug.Log($"Ending phase : {type}");
            
            onEnd?.Invoke();
            Event.Call($"{type}.{PhaseCallback.End}");
        }
    }
}