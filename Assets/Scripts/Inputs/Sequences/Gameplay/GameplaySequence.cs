using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewGameplaySequenceOperation", menuName = "Beau Tambour/Inputs/Sequences/Gameplay/Base")]
    public class GameplaySequence : InputSequence<GameplaySequenceKeys, GameplaySequenceElement>
    {
        public static Musician pickedMusician;

        [SerializeField] private AudioClip successAudio;
        [SerializeField] private AudioClip failAudio;
        
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            
            Event.Open(TempEvent.OnPartitionCompleted);
            Event.Open(TempEvent.OnAnyMusicianPicked);
            Event.Open<Musician>(TempEvent.OnMusicianPickedExtended);
        }
        
        protected override GameplaySequenceKeys Combine(GameplaySequenceKeys history, GameplaySequenceKeys key) => history | key;
        protected override void HandleOutcome(GameplaySequenceKeys history)
        {
            var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
            var audioSource = audioPool.RequestSingle();

            audioSource.clip = successAudio;
            audioSource.Play();
            
            if (history == (GameplaySequenceKeys.Start | GameplaySequenceKeys.PickMusician | GameplaySequenceKeys.End))
            {
                var attributes = pickedMusician.Prompt();

                Event.Call(TempEvent.OnAnyMusicianPicked);
                Event.CallLocal(TempEvent.OnMusicianPicked, pickedMusician);
                Event.Call(TempEvent.OnMusicianPickedExtended, pickedMusician);

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
        protected override void HandleFailure(GameplaySequenceKeys history, int groupIndex, GameplaySequenceKeys key)
        {
            if (key != GameplaySequenceKeys.End) return;
            
            var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
            var audioSource = audioPool.RequestSingle();

            audioSource.clip = failAudio;
            audioSource.Play();
        }
    }
}