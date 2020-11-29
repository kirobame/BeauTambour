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

        public Sequencer Sequencer => runtimeSequencer;
        
        [SerializeField] private int priority;
        [SerializeField] private OutcomeType mask;
        [SerializeField] private OutcomeType successionMask;
        [SerializeField] private bool shouldBeRemoved = true;
        
        [SerializeField] private Condition[] conditions;
        [SerializeField] private Sequencer sequencerPrefab;

        private Sequencer runtimeSequencer;

        public void BootUp()
        {
            if (!Application.isPlaying) return;

            var parent = Repository.GetSingle<Transform>(Parent.Outcomes);
            
            Debug.Log($"{name} - Runtime : {runtimeSequencer}");
            Debug.Log($"{name} - Prefab : {sequencerPrefab}");
            
            runtimeSequencer = Instantiate(sequencerPrefab, parent);
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