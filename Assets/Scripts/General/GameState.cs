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

        private static int finishedArcsCount;
        
        public static int BlockIndex { get; private set; }
        public static Language UsedLanguage { get; private set; }
        public static Note Note { get; private set; }

        public static bool validationMade;

        private static Dictionary<string, EventBoundDialogue> eventDialogues;

        public static void Bootup()
        {
            eventDialogues = new Dictionary<string, EventBoundDialogue>();
            speakers = new Dictionary<Actor, ISpeaker>();

            BlockIndex = -1;
            UsedLanguage = Language.Français;
            Note = new Note();

            Event.Open(GameEvents.OnBlockPassed);
            
            Event.Open<string>(GameEvents.OnNarrativeEvent);
            Event.Register<string>(GameEvents.OnNarrativeEvent, ReceiveNarrativeEvent);
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

                if (firstX > secondX) return 1;
                else return -1;
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

        public static void NotifyMusicianArcEnd()
        {
            finishedArcsCount++;
            if (finishedArcsCount == speakers.Count - 1) Event.Call<string>(GameEvents.OnNarrativeEvent, $"HarmonyBegun-{BlockIndex + 1}");
        }
        
        public static void PassBlock()
        {
            finishedArcsCount = 0;
            BlockIndex++;
            
            Event.Call(GameEvents.OnBlockPassed);
        }
    }
}