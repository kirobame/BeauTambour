using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public abstract class PlayerAction : SerializedMonoBehaviour
    {
        public int ActionLength => actionLength;
        
        protected Player player => Repository.Get<Player>();
        protected abstract ActionType type { get; }
        
        [SerializeField, Min(1)] protected int actionLength = 1;
        
        private bool hasBeenFreed;
        
        public void TryBeginExecution()
        {
            if (!player.IsActive || player.IsActionTypeClaimed(type) || !CanBeExecuted()) return;

            var rythmHandler = Repository.Get<RythmHandler>();
            if (rythmHandler.TryPlainEnqueue(Execute, actionLength))
            {
                player.ClaimActionType(type);
                rythmHandler.MakeStandardEnqueue(ResolveTime, actionLength);
            }
        }
        protected abstract bool CanBeExecuted();

        protected virtual void Execute(int beat, double offset)
        {
            if (beat == actionLength) hasBeenFreed = false;
        }
        protected virtual void ResolveTime(double time, double offset)
        {
            var freeingRange = actionLength - RythmHandler.StandardErrorMargin / Repository.Get<RythmHandler>().SecondsPerBeats;
            if (!hasBeenFreed && time + offset > freeingRange)
            {
                player.FreeActionType(type);
                hasBeenFreed = true;
            }
        }
    }
}