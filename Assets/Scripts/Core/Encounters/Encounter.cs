using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flux;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    [IconIndicator(7705900795600745325), CreateAssetMenu(fileName = "NewEncounter", menuName = "Beau Tambour/Encounter")]
    public class Encounter : ScriptableObject
    {
        #region Encapsulated Types

        [EnumAddress]
        public enum EventType
        {
            OnSuccess
        }

        #endregion
        
        public Note[][] Historic => historic.ToArray();
        public Outcome CurrentOutcome => results[outcomeAdvancement];
        public Interlocutor Interlocutor => interlocutor;
        
        public IReadOnlyList<Outcome> Outcomes => outcomes;
        [SerializeField] private Outcome[] firstOutcomes;
        [SerializeField] private Outcome[] outcomes;

        [SerializeField] private Character[] characters;
        [SerializeField] private Interlocutor initialInterlocutor;
        
        private HashSet<Outcome> runtimeOutcomes;
        private List<Outcome> results;

        private Stack<Note[]> historic = new Stack<Note[]>();
        private int outcomeAdvancement;

        private Interlocutor interlocutor;
        private int blockAdvancement;
        private int goToNextBlock;
        
        public void BootUp()
        {
            Event.Open(EventType.OnSuccess);
            
            outcomeAdvancement = -1;
            goToNextBlock = 0;

            interlocutor = initialInterlocutor;
            interlocutor.BootUp();
            foreach (var character in characters) character.BootUp();
            
            foreach (var outcome in firstOutcomes) outcome.BootUp();
            foreach (var outcome in outcomes) outcome.BootUp();
            runtimeOutcomes = new HashSet<Outcome>(outcomes);
            
            results = new List<Outcome>();
        }

        public void Start()
        {
            results.Add(firstOutcomes[0]);
            outcomeAdvancement = 0;
            
            results[0].Play(this, new Note[0]);
            results[0].Sequencer.OnCompletion += OnOutcomeDone;
        }
        public void Evaluate(Note[] notes)
        {
            if (outcomeAdvancement >= 0) return;
            
            var hook = Repository.GetSingle<Hook>(Reference.Hook);
            hook.StartCoroutine(EvaluationRoutine(notes));
        }

        private IEnumerator EvaluationRoutine(Note[] notes)
        {
            if (interlocutor.CurrentBlock.Emotion.Matches(notes))
            {
                if (interlocutor.isAtLastBlock)
                {
                    Debug.Log("Win !");
                    
                    Event.Call(EventType.OnSuccess);
                    goToNextBlock = 2;
                }
                else
                {
                    Debug.Log("Success !");
                    
                    Event.Call(EventType.OnSuccess);
                    goToNextBlock = 1;
                }
                
                yield return new WaitForSeconds(2);
                Debug.Log("Resuming");
            }

            PrintNotes(notes);
            if (!GetOutcomes(notes))
            {
                Debug.LogError("No outcome found !");
                Debug.Break();
                
                yield break;
            }
            
            PrintOutcomes();
            
            results[0].Play(this, notes);
            results[0].Sequencer.OnCompletion += OnOutcomeDone;
        }
        private void PrintNotes(Note[] notes)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Evaluating outcomes with :");
            for (var i = 0; i < notes.Length; i++)
            {
                builder.AppendLine($"---Note [{i}] :");
                foreach (var attribute in notes[i].Attributes) builder.AppendLine($"------{attribute}");
            }
            Debug.Log(builder.ToString());
        }
        private bool GetOutcomes(Note[] notes)
        {
            historic.Push(notes);
            results.Clear();
            
            foreach (var outcome in runtimeOutcomes)
            {
                if (!outcome.IsOperational(this, notes)) continue;
                results.Add(outcome);
            }

            if (!results.Any()) return false;
            
            results.Sort((first, second) =>
            {
                if (first.Priority == second.Priority) return 0;
                else if (first.Priority > second.Priority) return -1;
                else return 1;
            });
            
            if (results.Count > 3) results.RemoveRange(3, results.Count - 3);
            
            outcomeAdvancement = 0;
            return true;
        }
        private void PrintOutcomes()
        {
            var builder = new StringBuilder();
            
            builder.Clear();
            builder.AppendLine("Outcomes :");
            foreach (var result in results) builder.AppendLine($"---{result}");
            Debug.Log(builder.ToString());
        }

        private void OnOutcomeDone()
        {
            if (CurrentOutcome.ShouldBeRemoved) runtimeOutcomes.Remove(CurrentOutcome);
            CurrentOutcome.Sequencer.OnCompletion -= OnOutcomeDone;

            if (outcomeAdvancement + 1 < results.Count)
            {
                var succession = (CurrentOutcome.SuccessionMask | results[outcomeAdvancement + 1].Mask) == CurrentOutcome.SuccessionMask;
                if (!succession)
                {
                    End();
                    return;
                }
            }
            else
            {
                End();
                return;
            }
            
            void End()
            {
                if (goToNextBlock == 1)
                {
                    interlocutor.PushTrough();
                    goToNextBlock = 0;
                    
                    Debug.Log("Moving on to next phase");
                    
                    if (results.Count > 1) results.RemoveRange(1, results.Count - 1);
                    results.Add(firstOutcomes[interlocutor.blockIndex]);
                    
                    outcomeAdvancement++;
                    
                    CurrentOutcome.Play(this, historic.Peek());
                    CurrentOutcome.Sequencer.OnCompletion += OnOutcomeDone;

                    return;
                }
                else if (goToNextBlock == 2)
                {
                    Debug.Log("Encounter is done");
                    goToNextBlock = 0;
                }
                
                outcomeAdvancement = -1;

                var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                phaseHandler.SkipToNext();
            }
            
            outcomeAdvancement++;
            CurrentOutcome.Play(this, historic.Peek());
            CurrentOutcome.Sequencer.OnCompletion += OnOutcomeDone;
        }
    }
}