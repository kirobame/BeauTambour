using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewClearNotesOperation", menuName = "Beau Tambour/Operations/Clear Notes")]
    public class ClearNotesOperation : RythmOperation
    {
        protected override bool TryGetAction(out IRythmQueueable action)
        {
            action = new BeatAction(0, 0, Action);
            return true;
        }

        private void Action(int beat)
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);
            
            outcomePhase.ClearNotes();
        }
    }
}