using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewGameplaySequenceOperation", menuName = "Beau Tambour/Sequences/Gameplay")]
    public class GameplaySequence : InputSequence<GameplaySequenceKeys, GameplaySequenceElement>
    {
        public static Musician pickedMusician;
        //
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            
            Event.Open(TempEvent.OnPartitionCompleted);
            Event.Open(TempEvent.OnAnyMusicianPicked);
        }
        
        protected override GameplaySequenceKeys Combine(GameplaySequenceKeys history, GameplaySequenceKeys key) => history | key;
        protected override void HandleOutcome(GameplaySequenceKeys history)
        {
            Debug.Log("Handling outcome");
            if (history == (GameplaySequenceKeys.Start | GameplaySequenceKeys.PickMusician | GameplaySequenceKeys.End))
            {
                var attributes = pickedMusician.Prompt();
            
                Event.Call(TempEvent.OnAnyMusicianPicked);
                Event.CallLocal(TempEvent.OnMusicianPicked, pickedMusician);

                var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);

                outcomePhase.BeginNote();
                outcomePhase.EnqueueNoteAttributes(attributes);
            }
            else if (history == (GameplaySequenceKeys.Start | GameplaySequenceKeys.ClearNotes | GameplaySequenceKeys.End))
            {
                var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);
            
                outcomePhase.ClearNotes();
            }
            else if (history == (GameplaySequenceKeys.Start | GameplaySequenceKeys.CompletePartition | GameplaySequenceKeys.End))
            {
                var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);
            
                if (outcomePhase.NoteCount > 0) phaseHandler.SkipToNext();
            }
        }
    }
}