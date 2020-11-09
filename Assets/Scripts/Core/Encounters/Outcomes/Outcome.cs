using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEditor;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(1891504476101499824)]
    public class Outcome : ScriptableObject
    {
        public int Priority => priority;
        public OutcomeType Mask => mask;
        public OutcomeType SuccessionMask => successionMask;
        public bool ShouldBeRemoved => shouldBeRemoved;
        
        [SerializeField] private int priority;
        [SerializeField] private OutcomeType mask;
        [SerializeField] private OutcomeType successionMask;
        [SerializeField] private bool shouldBeRemoved;
        
        [SerializeField] private Condition[] conditions;
        [SerializeField] private Sequencer sequencerPrefab;

        public Sequencer Sequencer { get; private set; }

        public void BootUp()
        {
            if (!Application.isPlaying || Sequencer != null) return;
            
            var parent = Repository.GetSingle<Transform>(Parent.Outcomes);
            Sequencer = Instantiate(sequencerPrefab, parent);
        }

        public bool IsOperational(Encounter encounter, Note[] notes)
        {
            if (!conditions.Any()) return true;

            foreach (var condition in conditions) { if (!condition.IsMet(encounter, notes)) return false; }
            return true;
        }

        public void Play(Encounter encounter, Note[] notes) => Sequencer.Play();
    }
}