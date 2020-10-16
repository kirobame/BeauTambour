using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ShiftAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;

        [HideInInspector] public int direction;
        
        [SerializeField] private Token musiciansToken;

        protected override bool CanBeExecuted()
        {
            var roundHandler = Repository.Get<RoundHandler>();
            return roundHandler.CurrentType == PhaseType.Placement && direction != 0;
        }

        protected override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                var musicians = Repository.GetStack<Musician>(musiciansToken);
                foreach (var musician in musicians) musician.PrepareShift(direction);
            }
            base.Execute(beat, offset);
        }
        protected override void ResolveTime(double time, double offset)
        {
            base.ResolveTime(time, offset);
            
            var musicians = Repository.GetStack<Musician>(musiciansToken);
            foreach (var musician in musicians) musician.Shift(time / (actionLength - offset));
        }
    }
}