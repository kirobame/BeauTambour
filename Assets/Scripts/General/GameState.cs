using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public static class GameState
    {
        public static IEnumerable<ISpeaker> Speakers => speakers.Values;
        private static Dictionary<Actor, ISpeaker> speakers;
        
        public static int BlockIndex { get; private set; }
        public static Language UsedLanguage { get; private set; }
        public static Note Note { get; private set; }

        public static void Bootup()
        {
            speakers = new Dictionary<Actor, ISpeaker>();

            BlockIndex = -1;
            UsedLanguage = Language.Français;
            Note = new Note();

            Event.Open(GameEvents.OnBlockPassed);
        }

        public static void RegisterSpeakerForUse(ISpeaker speaker)
        {
            if (speakers.ContainsKey(speaker.Actor)) return;
            speakers.Add(speaker.Actor, speaker);
        }
        public static void UnregisterSpeakerForUse(Musician musician) => speakers.Remove(musician.Actor);

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

        public static void PassBlock()
        {
            BlockIndex++;
            Event.Call(GameEvents.OnBlockPassed);
        }
    }
}