using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class PadModule : Module<bool>
    {
        [SerializeField] private int direction;
        [SerializeField] private ShiftAction shiftAction;
        
        protected override void OnActionStarted(bool input)
        {
            shiftAction.direction = direction;
            shiftAction.TryBeginExecution();
        }

        protected override void OnAction(bool input) { }
        protected override void OnActionEnded(bool input) { }
    }

    public class ShiftAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;

        public int direction;
        
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
            if (beat == actionLength) direction = 0;
        }
        protected override void ResolveTime(double time, double offset)
        {
            var musicians = Repository.GetStack<Musician>(musiciansToken);
            foreach (var musician in musicians) musician.Shift(time / (actionLength - offset));
        }
    }
}