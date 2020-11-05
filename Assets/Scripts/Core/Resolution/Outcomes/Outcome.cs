using System;
using System.Linq;
using Flux;
using UnityEditor;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(1891504476101499824)]
    public class Outcome : ScriptableObject
    {
        public bool AllowsSuccession => allowsSuccession;
        public int Priority => priority;

        [SerializeField] private bool allowsSuccession;
        [SerializeField] private int priority;
        
        [SerializeField] private Condition[] conditions;
        [SerializeField] private Sequencer sequencerPrefab;

        public Sequencer Sequencer { get; private set; }

        public void BootUp()
        {
            if (!Application.isPlaying || Sequencer != null) return;
            
            var parent = Repository.GetSingle<Transform>(Parent.Outcomes);
            Sequencer = Instantiate(sequencerPrefab, parent);
        }

        public bool IsOperational(Note[] notes) => !conditions.Any() || conditions.All(condition => condition.IsMet(notes));
        public void Play(Note[] notes) => Sequencer.Play();

        private void OnDisable()
        {
            Debug.Log(1);
        }

        private void OnDestroy()
        {
            
        }
    }
}