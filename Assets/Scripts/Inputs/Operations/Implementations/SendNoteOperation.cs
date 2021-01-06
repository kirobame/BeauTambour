using System;
using System.Collections;
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
            
            GameState.validationMade = true;
            hook.StartCoroutine(ActivationRoutine());
        }

        private IEnumerator ActivationRoutine()
        {
            Event.Call(GameEvents.OnNoteValidation);
            yield return new WaitForSeconds(0.75f);
            
            GameState.Note.speaker.PlayMelodyFor(GameState.Note.emotion);
        }
    }
}