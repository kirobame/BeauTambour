using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class EndAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;

        protected override void OnActionStarted(bool input) { }
        protected override void OnActionEnded(bool input) => TryBeginExecution();

        protected override bool CanBeExecuted() => Repository.Get<Player>().phase == 2;

        protected override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                Debug.Log("Ending acting phase !");
                
                var roundHandler = Repository.Get<RoundHandler>();
                var actingPhase = roundHandler[PhaseType.Acting] as Phase;
                
                actingPhase.Add(actingPhase.Length - actingPhase.Advancement);
            }
            base.Execute(beat, offset);
        }
    }
}