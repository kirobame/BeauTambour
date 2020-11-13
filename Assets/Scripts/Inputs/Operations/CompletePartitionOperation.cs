using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewCompletePartitionOperation", menuName = "Beau Tambour/Operations/Complete Partition")]
    public class CompletePartitionOperation : RythmOperation
    {
        public override void Initialize(OperationHandler operationHandler)
        {
            base.Initialize(operationHandler);
            Event.Open(TempEvent.OnPartitionCompleted);
        }

        protected override bool TryGetAction(out IRythmQueueable action)
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);

            if (outcomePhase.NoteCount > 0)
            {
                action = new BeatAction(0, 0, Action);
                return true;
            }
            else
            {
                action = null;
                return false;
            }
        }
        //
        private void Action(int beat)
        {
            Event.Call(TempEvent.OnPartitionCompleted);
            
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            phaseHandler.SkipToNext();
        }
    }
}