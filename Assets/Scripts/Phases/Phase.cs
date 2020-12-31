using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public abstract class Phase : MonoBehaviour, IPhase
    {
        public abstract PhaseCategory Category { get; }
        
        protected PhaseHandler owner;

        protected virtual void Awake()
        {
            Event.Open($"{Category}.{PhaseCallback.Start}");
            Event.Open($"{Category}.{PhaseCallback.End}");
        }
        void Start()
        {
            owner = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            owner.TryRegister(this);
        }

        public virtual void Begin() => Event.Call($"{Category}.{PhaseCallback.Start}");
        public virtual void End() => Event.Call($"{Category}.{PhaseCallback.End}");
        
        public virtual void Pause() { }
        public virtual void Resume() { }
    }
}