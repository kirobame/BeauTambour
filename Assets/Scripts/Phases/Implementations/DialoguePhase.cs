using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class DialoguePhase : Phase
    {
        public override PhaseCategory Category => PhaseCategory.Dialogue;

        public override void Begin()
        {
            base.Begin();
            
            var dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            var dialogue = GameState.Note.speaker.GetDialogue(GameState.Note.emotion);
            
            dialogueHandler.Enqueue(dialogue);
        }
        public override void End()
        {
            base.End();
        }
    }
}