using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class PhaseHandler : MonoBehaviour
    {
        public IReadOnlyDictionary<PhaseCategory, IPhase> Phases => phases;
        private Dictionary<PhaseCategory, IPhase> phases;

        public IPhase CurrentPhase => phases[currentCategory];
        public PhaseCategory CurrentCategory => currentCategory;
        private PhaseCategory currentCategory;
        
        void Awake()
        {
            phases = new Dictionary<PhaseCategory, IPhase>();
            currentCategory = PhaseCategory.None;

            Repository.Reference(this, References.PhaseHandler);
        }

        public bool TryRegister(IPhase phase)
        {
            if (phases.ContainsKey(phase.Category)) return false;
            
            phases.Add(phase.Category, phase);
            return true;
        }
        public bool TryUnregister(IPhase phase)
        {
            if (!phases.ContainsKey(phase.Category) || phases[phase.Category] != phase) return false;

            phases.Remove(phase.Category);
            return true;
        }

        public void Play(PhaseCategory category)
        {
            if (!phases.ContainsKey(category)) return;
            if (currentCategory != PhaseCategory.None) CurrentPhase.End();
            
            phases[category].Begin();
            currentCategory = category;
        }
    }
}