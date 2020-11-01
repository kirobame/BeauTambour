using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour
{
    public class Encounter : ScriptableObject
    {
        [SerializeField] private OutcomePort[] ports;
        [SerializeField] private Outcome[] outcomes;

        private HashSet<Outcome> runtimeOutcomes;
        private Dictionary<NoteAttributeType, List<OutcomePort>> registry;
        
        void OnEnable()
        {
            runtimeOutcomes = new HashSet<Outcome>(outcomes);
            registry = new Dictionary<NoteAttributeType, List<OutcomePort>>();
            
            foreach (var port in ports)
            {
                foreach (var key in port.Keys)
                {
                    if (registry.TryGetValue(key, out var list) && !list.Contains(port)) list.Add(port);
                    else registry.Add(key, new List<OutcomePort>() {port});
                }
            }
        }

        public void Evaluate(IEnumerable<Note> notes)
        {
            foreach (var port in ports) port.Reboot();
            foreach (var note in notes)
            {
                foreach (var attribute in note.Attributes)
                {
                    if (!registry.TryGetValue(attribute.Type, out var list)) continue;
                    foreach (var port in list) port.TryAdvance(attribute);
                }
            }

            var highestPriority = int.MinValue;
            var result = default(Outcome);

            foreach (var outcome in runtimeOutcomes)
            {
                if (!outcome.IsOperational) continue;

                if (highestPriority < outcome.Priority)
                {
                    highestPriority = outcome.Priority;
                    result = outcome;
                }
            }

            if (highestPriority == int.MinValue)
            {
                Debug.LogError("No Valid Outcome Found !");
                Debug.Break();

                return;
            }
            
            result.Sequencer.Play();
        }
    }
}