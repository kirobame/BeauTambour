using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public abstract class PlayerAction : Module<bool>
    {
        public int ActionLength => actionLength;
        
        protected Player player => Repository.Get<Player>();
        protected abstract ActionType type { get; }

        [SerializeField, Min(1)] protected int actionLength = 1;
        
        private bool hasBeenFreed;

        protected override void OnActionStarted(bool input) => TryBeginExecution();
        protected override void OnAction(bool input) { }
        protected override void OnActionEnded(bool input) { }

        protected void TryBeginExecution()
        {
            Debug.Log($"For : {transform.parent.name}{name} -> Claim : {player.IsActionTypeClaimed(type)} / Execution : {!CanBeExecuted()}");
            
            if (!player.IsActive || player.IsActionTypeClaimed(type) || !CanBeExecuted()) return;

            var rythmHandler = Repository.Get<RythmHandler>();
            if (rythmHandler.TryPlainEnqueue(Execute, actionLength))
            {
                Debug.Log($"RIGHT ! {transform.parent.name}{name}'s Action was on time");
                
                player.ClaimActionType(type);
                rythmHandler.MakeStandardEnqueue(ResolveTime, actionLength);
            }
            else Debug.Log($"WRONG ! {transform.parent.name}{name}'s Action was not on time");
        }
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