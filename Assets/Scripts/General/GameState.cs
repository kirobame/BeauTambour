using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public static class GameState
    {
        public static IEnumerable<ISpeaker> Speakers => speakers.Values;
        private static Dictionary<Actor, ISpeaker> speakers;

        public static int FinishedArcsCount { get; private set; }
        private static bool arcsMinimumCompletion;
        
        public static int BlockIndex { get; private set; }
        public static Language UsedLanguage { get; private set; }
        public static Note Note { get; private set; }

        public static bool validationMade;

        private static Dictionary<string, EventBoundDialogue> eventDialogues;

        public static void Bootup(int blockIndex)
        {
            Debug.Log("BOOTING UP GAME STATE");
            
            FinishedArcsCount = 0;
            arcsMinimumCompletion = false;
            
            eventDialogues = new Dictionary<string, EventBoundDialogue>();
            speakers = new Dictionary<Actor, ISpeaker>();

            BlockIndex = blockIndex - 1;
            UsedLanguage = Language.Français;
            Note = new Note();

            Event.Open(GameEvents.OnLanguageChanged);
            Event.Open(GameEvents.OnBlockPassed);
            Event.Open(GameEvents.OnEncounterEnd);
            
            Event.Open<string>(GameEvents.OnNarrativeEvent);
            Event.Register<string>(GameEvents.OnNarrativeEvent, ReceiveNarrativeEvent);
        }

        public static void ChangeLanguage(Language language)
        {
            if (language == UsedLanguage) return;

            UsedLanguage = language;
            Event.Call(GameEvents.OnLanguageChanged);
        }

        public static void RegisterSpeakerForUse(ISpeaker speaker)
        {
            if (speakers.ContainsKey(speaker.Actor)) return;
            speakers.Add(speaker.Actor, speaker);
        }
        public static void UnregisterSpeakerForUse(ISpeaker speaker) => speakers.Remove(speaker.Actor);

        public static bool TryGetSpeaker(Actor actor, out ISpeaker musician) => speakers.TryGetValue(actor, out musician);
        public static ISpeaker GetSpeaker(Actor actor) => speakers[actor];
        public static ISpeaker[] GetSortedSpeakers()
        {
            var list = new List<ISpeaker>(Speakers);
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
            
            if (!arcsMinimumCompletion && FinishedArcsCount == speakers.Count - 1)
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
            var encounter = Repository.GetSingle<Encounter>(References.Encounter);
            UnregisterSpeakerForUse(encounter.Interlocutor);
            
            FinishedArcsCount = 0;
            arcsMinimumCompletion = false;

            BlockIndex++;

            if (BlockIndex >= encounter.BlockCount)
            {
                var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
                phaseHandler.SetActive(false);

                Event.Register<Dialogue>(GameEvents.OnDialogueFinished, dialogue => OnEnd());
            }
            else Event.Call(GameEvents.OnBlockPassed);
        }

        private static void OnEnd() => Event.Call(GameEvents.OnEncounterEnd);
    }
}