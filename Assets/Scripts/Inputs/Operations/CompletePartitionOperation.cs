using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewCompletePartitionOperation", menuName = "Beau Tambour/Operations/Complete Partition")]
    public class CompletePartitionOperation : RythmOperation
    {
        protected override bool TryGetAction(out IRythmQueueable action)
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);

            if (outcomePhase.NoteCount > 0)
            {
                action = new BeatAction(1, 1, Action);
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
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            phaseHandler.SkipToNext();
        }
    }
}