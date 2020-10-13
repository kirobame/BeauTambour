using System;
using System.Collections.Generic;
using System.Linq;
using Ludiq.PeekCore.ReflectionMagic;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ResolutionPhase : MonoBehaviour, IPhase
    {
        public event Action OnStart;
        public event Action OnEnd;

        public PhaseType Type => PhaseType.Resolution;

        [ShowInInspector, ReadOnly] private SortedList<int, List<IResolvable>> resolvables;

        void Awake()
        {
            var comparer = Comparer<int>.Create((first, second) =>
            {
                if (first == second) return 0;
                else if (first < second) return -1;
                else return 1;
            });
            resolvables = new SortedList<int, List<IResolvable>>(comparer);
        }

        public bool Advance() => !resolvables.Any();

        public void Begin()
        {
            var count = 0;
            foreach (var resolvableList in resolvables.Values) count += resolvableList.Count;
            
            var rythmHandler = Repository.Get<RythmHandler>();
            rythmHandler.MakePlainEnqueue(Dequeue, count);
        }
        public void End() => resolvables.Clear();

        public bool TryEnqueue(IResolvable resolvable)
        {
            var roundHandler = Repository.Get<RoundHandler>();
            if (!roundHandler.CurrentType.IsActingPhase()) return false;

            if (resolvables.ContainsKey(resolvable.Priority)) resolvables[resolvable.Priority].Add(resolvable);
            else resolvables.Add(resolvable.Priority, new List<IResolvable>() {resolvable});
            
            return true;
        }

        private void Dequeue(int beat, double offset)
        {
            if (!resolvables.Any()) return;

            var list = resolvables.First().Value;
            var resolvable = list.First();
            
            resolvable.Resolve();

            list.RemoveAt(0);
            if (list.Count == 0) resolvables.Remove(resolvable.Priority);
        }
    }
}