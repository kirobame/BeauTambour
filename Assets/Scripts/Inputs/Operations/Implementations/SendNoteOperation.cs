using System;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSendNoteOperation", menuName = "Beau Tambour/Operations/Send Note")]
    public class SendNoteOperation : PhaseBoundOperation
    {
        protected override void RelayedOnStart(EventArgs inArgs)
        {
            if (GameState.validationMade) return;

            Event.Call(GameEvents.OnNoteValidation);
            GameState.Note.speaker.PlayMelodyFor(GameState.Note.emotion);
            //
            GameState.validationMade = true;
        }
    }
}