using Flux;
using UnityEngine;

namespace BeauTambour
{
    public abstract class Phase : MonoBehaviour, IPhase
    {
        public abstract PhaseCategory Category { get; }
        
        private PhaseHandler owner;
        
        void Start()
        {
            owner = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            owner.TryRegister(this);
        }

        public abstract void Begin();
        public abstract void End();
    }
}