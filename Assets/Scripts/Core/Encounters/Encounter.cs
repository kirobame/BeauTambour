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
        public Note[][] Historic => historic.ToArray();
        public Outcome CurrentOutcome => results[outcomeAdvancement];
        
        public IReadOnlyList<Outcome> Outcomes => outcomes;
        [SerializeField] private Outcome[] outcomes;

        public IEnumerable<Character> Characters => characterInputs.Select(input => input.Character);
        [SerializeField] private CharacterInput[] characterInputs;

        private HashSet<Outcome> runtimeOutcomes;
        private List<Outcome> results;

        private Stack<Note[]> historic;
        private int outcomeAdvancement;
        
        public void BootUp()
        {
            outcomeAdvancement = -1;
            
            foreach (var outcome in outcomes) outcome.BootUp();
            foreach (var characterInput in characterInputs) characterInput.Execute();
            
            runtimeOutcomes = new HashSet<Outcome>(outcomes);
            results = new List<Outcome>();
        }

        public void Evaluate(Note[] notes)
        {
            if (outcomeAdvancement >= 0) return;
            historic.Push(notes);
            
            results.Clear();
            foreach (var outcome in runtimeOutcomes)
            {
                if (!outcome.IsOperational(this, notes)) continue;
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
            
            results[0].Play(this, notes);
            results[0].Sequencer.OnCompletion += OnOutcomeDone;
        }

        private void OnOutcomeDone()
        {
            runtimeOutcomes.Remove(CurrentOutcome);
            CurrentOutcome.Sequencer.OnCompletion -= OnOutcomeDone;

            var succession = (CurrentOutcome.SuccessionMask | results[outcomeAdvancement + 1].Mask) == CurrentOutcome.SuccessionMask;
            if (outcomeAdvancement + 1 >= results.Count || !succession)
            {
                outcomeAdvancement = -1;
                return;
            }
            
            outcomeAdvancement++;
            CurrentOutcome.Play(this, historic.Peek());
            CurrentOutcome.Sequencer.OnCompletion += OnOutcomeDone;
        }
    }
}