using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewPickMusicianOperation", menuName = "Beau Tambour/Operations/Pick Musician")]
    public class PickMusicianOperation : RythmOperation
    {
        [SerializeField] private Musician musician;
        
        protected override bool TryGetAction(out IRythmQueueable action)
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);

            if (outcomePhase.IsNoteBeingProcessed)
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

        private void Action(int beat)
        {
            var attributes = musician.Prompt();
                
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);
            
            outcomePhase.EnqueueNoteAttributes(attributes);
            outcomePhase.CompleteNote();
        }
    }
}