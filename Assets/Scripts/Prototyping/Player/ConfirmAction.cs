using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ConfirmAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;
        
        protected override bool CanBeExecuted()
        {
            var roundHandler = Repository.Get<RoundHandler>();
            return roundHandler.CurrentType == PhaseType.Placement || roundHandler.CurrentType == PhaseType.Setup;
        }

        protected override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                var roundHandler = Repository.Get<RoundHandler>();
                
                var phase = roundHandler.Current as Phase;
                phase.Add(phase.Length - phase.Advancement);
            }
            base.Execute(beat, offset);
        }
    }
}