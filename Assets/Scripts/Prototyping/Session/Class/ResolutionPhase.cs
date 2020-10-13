using System.Collections.Generic;
using Ludiq.PeekCore.ReflectionMagic;
using Orion;

namespace BeauTambour.Prototyping
{
    public class ResolutionPhase : Phase
    {
        private SortedList<int, List<IResolvable>> resolvables;

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

        public bool TryEnqueue(IResolvable resolvable)
        {
            var roundHandler = Repository.Get<RoundHandler>();
            if (roundHandler.CurrentType != PhaseType.Acting) return false;

            if (resolvables.ContainsKey(resolvable.Priority)) resolvables[resolvable.Priority].Add(resolvable);
            else resolvables.Add(resolvable.Priority, new List<IResolvable>() {resolvable});
            
            return true;
        }

        public override void Begin()
        {
            foreach (var resolvableList in resolvables.Values)
            {
                foreach (var resolvable in resolvableList) resolvable.Resolve();
            }
            resolvables.Clear();
        
            base.Begin();
        }
    }
}