using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewPickMusicianOperation", menuName = "Beau Tambour/Operations/Pick Musician")]
    public class PickMusicianOperation : RythmOperation
    {
        [SerializeField] private Musician musician;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            
            Event.Open(TempEvent.OnAnyMusicianPicked);
            Event.Open(TempEvent.OnMusicianPicked, musician);
        }

        protected override bool TryGetAction(out IRythmQueueable action)
        {
            action = new BeatAction(0, 0, Action);
            return true;
        }

        private void Action(int beat)
        {
            var attributes = musician.Prompt();
            
            Event.Call(TempEvent.OnAnyMusicianPicked);
            Event.CallLocal(TempEvent.OnMusicianPicked, musician);

            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);

            outcomePhase.BeginNote();
            outcomePhase.EnqueueNoteAttributes(attributes);
        }
    }
}