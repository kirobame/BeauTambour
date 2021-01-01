using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class DialoguePhase : Phase
    {
        public override PhaseCategory Category => PhaseCategory.Dialogue;

        private bool hasBeenBootedUp;

        public override void Begin()
        {
            base.Begin();

            if (!hasBeenBootedUp)
            {
                Event.Call<string>(GameEvents.OnNarrativeEvent, "Start");
                hasBeenBootedUp = true;
            }
            else
            {
                var dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
                var dialogues = GameState.Note.speaker.GetDialogues(GameState.Note.emotion);

                foreach (var dialogue in dialogues) dialogueHandler.Enqueue(dialogue);
            }
        }
    }
}