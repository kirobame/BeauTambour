using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public abstract class PlayerAction : Module<bool>
    {
        protected Player player => Repository.Get<Player>();
        protected abstract ActionType type { get; }

        [SerializeField, Min(1)] protected int actionLength = 1;
        
        private bool hasBeenFreed;

        protected override void OnActionStarted(bool input)
        {
            if (!player.IsActive || player.IsActionTypeClaimed(type) || !CanBeExecuted()) return;

            var rythmHandler = Repository.Get<RythmHandler>();
            if (rythmHandler.TryPlainEnqueue(Execute, actionLength))
            {
                player.ClaimActionType(type);
                rythmHandler.MakeStandardEnqueue(ResolveTime, actionLength);
            }
        }
        protected override void OnAction(bool input) { }
        protected override void OnActionEnded(bool input) { }

        protected abstract bool CanBeExecuted();

        protected virtual void Execute(int beat, double offset)
        {
            if (beat == actionLength) hasBeenFreed = false;
        }
        protected virtual void ResolveTime(double time, double offset)
        {
            if (!hasBeenFreed && time + offset > actionLength - RythmHandler.StandardErrorMargin)
            {
                player.FreeActionType(type);
                hasBeenFreed = true;
            }
        }
    }
}