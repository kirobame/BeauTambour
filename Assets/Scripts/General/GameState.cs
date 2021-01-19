using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public static class GameState
    {
        public static IEnumerable<Character> ActiveSpeakers => activeSpeakers.Values;
        private static Dictionary<Actor, Character> activeSpeakers;

        public static int FinishedArcsCount { get; private set; }
        private static bool arcsMinimumCompletion;
        
        public static bool PhaseStop { get; private set; }
        public static int BlockIndex { get; private set; }
        public static Note Note { get; private set; }

        public static Language UsedLanguage { get; private set; }
        
        public static bool validationMade;
        private static Dictionary<string, EventBoundDialogue> eventDialogues;

        private static bool hasBeenBootedUp;

        public static void Bootup(int blockIndex)
        {
            hasBeenBootedUp = false;
            
            FinishedArcsCount = 0;
            arcsMinimumCompletion = false;
            
            eventDialogues = new Dictionary<string, EventBoundDialogue>();
            activeSpeakers = new Dictionary<Actor, Character>();

            PhaseStop = false;
            BlockIndex = blockIndex - 1;
            Note = new Note();
            
            UsedLanguage = Language.Français;
            
            Event.Open(GameEvents.OnLanguageChanged);
            Event.Open(GameEvents.OnBlockPassed);
            Event.Open(GameEvents.OnEncounterEnd);
            
            Event.Open<string>(GameEvents.OnNarrativeEvent);
            Event.Register<string>(GameEvents.OnNarrativeEvent, ReceiveNarrativeEvent);

            Event.Register(GameEvents.OnCurtainRaised, OnCurtainRaised);
        }

        public static void ChangeLanguage(Language language)
        {
            if (language == UsedLanguage) return;

            UsedLanguage = language;
            Event.Call(GameEvents.OnLanguageChanged);
        }

        public static void RegisterSpeakerForUse(Character speaker)
        {
            if (activeSpeakers.ContainsKey(speaker.Actor)) return;
            activeSpeakers.Add(speaker.Actor, speaker);
        }
        public static void UnregisterSpeakerForUse(Character speaker) => activeSpeakers.Remove(speaker.Actor);

        public static bool TryGetSpeaker(Actor actor, out Character musician) => activeSpeakers.TryGetValue(actor, out musician);
        public static Character GetSpeaker(Actor actor) => activeSpeakers[actor];
        public static Character[] GetSortedSpeakers()
        {
            var list = new List<Character>(ActiveSpeakers);
            list.Sort((first, second) =>
            {
                var firstX = first.RuntimeLink.transform.position.x;
                var secondX = second.RuntimeLink.transform.position.x;

                if (firstX > secondX) return -1;
                else return 1;
            });

            return list.ToArray();
        }

        public static void AddEventBoundDialogue(EventBoundDialogue eventDialogue)
        {
            if (eventDialogues.ContainsKey(eventDialogue.Key)) return;
            eventDialogues.Add(eventDialogue.Key, eventDialogue);
        }
        static void ReceiveNarrativeEvent(string message)
        {
            if (!eventDialogues.ContainsKey(message)) return;

            var dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            dialogueHandler.Enqueue(eventDialogues[message].GetDialogue());
        }

        public static bool NotifyMusicianArcEnd(out Dialogue dialogue)
        {
            FinishedArcsCount++;
            
            if (!arcsMinimumCompletion && FinishedArcsCount == activeSpeakers.Count - 1)
            {
                Event.Call(GameEvents.OnInterlocutorConvinced);
                
                var encounter = Repository.GetSingle<Encounter>(References.Encounter);
                RegisterSpeakerForUse(encounter.Interlocutor);
                
                dialogue = eventDialogues[$"Block.{BlockIndex + 1}"].GetDialogue();
                arcsMinimumCompletion = true;
                
                return true;
            }
            else
            {
                dialogue = null;
                return false;
            }
        }
        
        public static void PassBlock()
        {
            if (!hasBeenBootedUp) hasBeenBootedUp = true;
            else PhaseStop = true;
            
            FinishedArcsCount = 0;
            arcsMinimumCompletion = false;

            BlockIndex++;

            var encounter = Repository.GetSingle<Encounter>(References.Encounter);
            if (BlockIndex >= encounter.BlockCount)
            {
                var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
                phaseHandler.SetActive(false);

                Event.Register<Dialogue>(GameEvents.OnDialogueFinished, dialogue => OnEnd());
            }
            else Event.Call(GameEvents.OnBlockPassed);
        }

        private static void OnCurtainRaised() => PhaseStop = false;
        private static void OnEnd() => Event.Call(GameEvents.OnEncounterEnd);
    }
}