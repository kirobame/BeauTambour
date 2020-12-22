using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public static class GameState
    {
        public static IEnumerable<Musician> Musicians => musicians.Values;
        private static Dictionary<Actor, Musician> musicians;
        
        public static int BlockIndex { get; private set; }
        public static Language UsedLanguage { get; private set; }
        public static Note Note { get; private set; }

        public static void Bootup()
        {
            musicians = new Dictionary<Actor, Musician>();

            BlockIndex = -1;
            UsedLanguage = Language.Français;
            Note = new Note();

            Event.Open(GameEvents.OnBlockPassed);
        }

        public static void RegisterMusicianForUse(Musician musician)
        {
            if (musicians.ContainsKey(musician.Actor)) return;
            musicians.Add(musician.Actor, musician);
        }
        public static void UnregisterMusicianForUse(Musician musician) => musicians.Remove(musician.Actor);

        public static bool TryGetMusician(Actor actor, out Musician musician) => musicians.TryGetValue(actor, out musician);
        public static Musician GetMusician(Actor actor) => musicians[actor];
        public static Musician[] GetSortedMusicians()
        {
            var list = new List<Musician>(Musicians);
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