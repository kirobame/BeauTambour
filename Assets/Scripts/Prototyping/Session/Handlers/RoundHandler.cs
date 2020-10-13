using Orion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class RoundHandler : SerializedMonoBehaviour, IBootable
    {
        public event Action OnRoundLoop;
        public event Action<PhaseType> OnPhaseChange;
        
        //--------------------------------------------------------------------------------------------------------------
        
        public PhaseType CurrentType { get; private set; }

        public IReadOnlyList<IPhase> Phases => phases;
        public IPhase this[PhaseType type] => phases.Find(phase => phase.Type == type);
    
        //--------------------------------------------------------------------------------------------------------------

        [SerializeField] private int bootUpPriority;
    
        [Space, SerializeField] private List<IPhase> phases = new List<IPhase>();
        private int advancement = -2;

        //--------------------------------------------------------------------------------------------------------------

        int IBootable.Priority => bootUpPriority;
    
        //--------------------------------------------------------------------------------------------------------------

        void Start() => Repository.Get<RythmHandler>().OnBeat += OnBeat;

        public void BootUp()
        {
            advancement = 0;
            phases.First().Begin();
        }
        public void ShutDown() { }

        private void OnBeat(double beat)
        {
            Debug.Log(CurrentType);
            
            var shouldMoveToNext = phases[advancement].Advance();
            if (shouldMoveToNext)
            {
                phases[advancement].End();

                if (advancement + 1 >= phases.Count)
                {
                    advancement = 0;
                    OnRoundLoop?.Invoke();
                }
                else advancement++;

                CurrentType = phases[advancement].Type;
                phases[advancement].Begin();

                OnPhaseChange?.Invoke(CurrentType);
            }
        }
    }
}