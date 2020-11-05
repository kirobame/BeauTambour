using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour
{
    [IconIndicator(7705900795600745325), CreateAssetMenu(fileName = "NewEncounter", menuName = "Beau Tambour/Resolution/Encounter")]
    public class Encounter : ScriptableObject
    {
        [SerializeField] private OutcomePort[] ports;
        [SerializeField] private Outcome[] outcomes;

        private HashSet<Outcome> runtimeOutcomes;
        private Dictionary<NoteAttributeType, List<OutcomePort>> registry;
        private List<Outcome> results;

        private Note[] currentNotes;
        private int outcomeAdvancement;
        
        public void BootUp()
        {
            outcomeAdvancement = -1;
            
            foreach (var port in ports) port.BootUp();
            foreach (var outcome in outcomes) outcome.BootUp();
            
            runtimeOutcomes = new HashSet<Outcome>(outcomes);
            registry = new Dictionary<NoteAttributeType, List<OutcomePort>>();
            results = new List<Outcome>();
            
            foreach (var port in ports)
            {
                foreach (var key in port.Keys)
                {
                    if (registry.TryGetValue(key, out var list))
                    {
                        if (!list.Contains(port)) list.Add(port);
                    }
                    else registry.Add(key, new List<OutcomePort>() {port});
                }
            }
        }

        public void Evaluate(Note[] notes)
        {
            if (outcomeAdvancement >= 0) return;
            currentNotes = notes;
            
            foreach (var port in ports) port.Reboot();
            foreach (var note in notes) note.Evaluate(registry);
            
            results.Clear();
            foreach (var outcome in runtimeOutcomes)
            {
                if (!outcome.IsOperational(notes)) continue;
                results.Add(outcome);
            }

            if (!results.Any())
            {
                Debug.LogError("No outcome found !");
                Debug.Break();

                return;
            }
            
            results.Sort((first, second) =>
            {
                if (first.Priority == second.Priority) return 0;
                else if (first.Priority > second.Priority) return -1;
                else return 1;
            });
            
            outcomeAdvancement = 0;
            
            results[0].Play(notes);
            results[0].Sequencer.OnCompletion += OnOutcomeDone;
        }

        private void OnOutcomeDone()
        {
            runtimeOutcomes.Remove(results[outcomeAdvancement]);
            results[outcomeAdvancement].Sequencer.OnCompletion -= OnOutcomeDone;
            
            if (outcomeAdvancement + 1 >= results.Count || !results[outcomeAdvancement].AllowsSuccession)
            {
                outcomeAdvancement = -1;
                return;
            }
            
            outcomeAdvancement++;
            results[outcomeAdvancement].Play(currentNotes);
            results[outcomeAdvancement].Sequencer.OnCompletion += OnOutcomeDone;
        }
    }
}