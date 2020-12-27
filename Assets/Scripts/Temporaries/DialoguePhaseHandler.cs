using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class DialoguePhaseHandler : MonoBehaviour
    {
        [SerializeField] private Musician musician;
        
        [ContextMenuItem("Continue", "Continue")]
        [SerializeField, ContextMenuItem("Display", "Display")] private Emotion emotion;

        public void Display()
        {
            GameState.Note.speaker = musician;
            GameState.Note.emotion = emotion;
            
            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            phaseHandler.Play(PhaseCategory.Dialogue);
        }
        public void Continue()
        {
            var dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            dialogueHandler.Continue();
        }
    }
}