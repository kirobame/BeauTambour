using System.Collections.Generic;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class PhaseHandler : MonoBehaviour
    {
        #region Encapsuled Types

        [EnumAddress]
        public enum EvenType
        {
            OnChange,
            OnLoop,
        }
        #endregion

        public PhaseType CurrentType => Current.Type;
        public IPhase Current => runtimePhases[advancement];
        
        public IReadOnlyList<IPhase> Phases => runtimePhases;
        public IPhase this[PhaseType type] => runtimePhases.Find(phase => phase.Type == type);
        
        [SerializeField] private Phase[] phases;

        private List<IPhase> runtimePhases;
        private int advancement = -1;

        void Awake()
        {
            runtimePhases = new List<IPhase>(phases);

            Event.Open<IPhase>(EvenType.OnChange);
            Event.Open(EvenType.OnLoop);
        }

        public void BootUp()
        {
            advancement = -1;
            Next();
        }
        public void ShutDown()
        {
            Current.onEnd -= OnCurrentEnd;
            advancement = -1;
        }

        private void Next()
        {
            if (advancement + 1 >= runtimePhases.Count)
            {
                advancement = -1;
                Event.Call(EvenType.OnLoop);
            }
            
            advancement++;
            Current.Begin();
            Current.onEnd += OnCurrentEnd;
        }
        private void OnCurrentEnd()
        {
            Current.onEnd -= OnCurrentEnd;
            Next();

            Event.Call<IPhase>(EvenType.OnChange, Current);
        }

        public void SwitchTo(PhaseType type) => SwitchTo(this[type]);
        public void SwitchTo(IPhase phase)
        {
            var index = runtimePhases.IndexOf(phase);
            if (index == -1) return;
            
            Current.onEnd -= OnCurrentEnd;
            advancement = index - 1;
            
            Next();
        }
    }
}