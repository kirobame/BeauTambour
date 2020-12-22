using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class DialoguePhase : Phase
    {
        public override PhaseCategory Category => PhaseCategory.Dialogue;

        public override void Begin()
        {
            var dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            var dialogue = GameState.Note.musician.GetDialogue(GameState.Note.emotion);
            
            dialogueHandler.Play(dialogue);
        }
        public override void End()
        {
            
        }
    }
}