using Flux;

namespace BeauTambour
{
    public class ValidationPhase : Phase
    {
        public override PhaseCategory Category => PhaseCategory.NoteValidation;

        protected override void Awake()
        {
            base.Awake();
            
            Event.Open(GameEvents.OnNoteValidation);
            Event.Open(GameEvents.OnNoteValidationDone);
            Event.Register(GameEvents.OnNoteValidationDone, OnNoteValidationDone);
            
            Event.Open(GameEvents.OnNoteDeletion);
            Event.Open(GameEvents.OnNoteDeletionDone);
            Event.Register(GameEvents.OnNoteDeletionDone, OnNoteDeletionDone);
        }

        public override void Begin()
        {
            base.Begin();
            GameState.validationMade = false;
        }

        void OnNoteValidationDone()
        {
            owner.Play(PhaseCategory.Dialogue);
        }

        void OnNoteDeletionDone()
        {
            owner.Play(PhaseCategory.SpeakerSelection);
        }
    }
}