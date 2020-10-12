using System.Collections.Generic;
using Orion;

namespace BeauTambour.Prototyping
{
    public class ResolutionPhase : Phase
    {
        private SortedList<int, IResolvable> resolvables;

        void Awake()
        {
            var comparer = Comparer<int>.Create((first, second) =>
            {
                if (first == second) return 0;
                else if (first < second) return -1;
                else return 1;
            });
            resolvables = new SortedList<int, IResolvable>(comparer);
        }

        public bool TryEnqueue(IResolvable resolvable)
        {
            var roundHandler = Repository.Get<RoundHandler>();
            if (roundHandler.CurrentType != PhaseType.Acting) return false;
        
            resolvables.Add(resolvable.Priority, resolvable);
            return true;
        }

        public override void Begin()
        {
            foreach (var resolvable in resolvables) resolvable.Value.Resolve();
            resolvables.Clear();
        
            base.Begin();
        }
    }
}