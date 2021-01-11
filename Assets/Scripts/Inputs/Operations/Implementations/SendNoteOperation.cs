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

            var emotion = GameState.Note.emotion;
            Debug.Log("---> A");
            
            if (GameState.Note.speaker.IsValid(emotion, out var selection, out var branches))
            {
                if (branches == 0 && GameState.Note.speaker is Musician musician) Event.Call<Musician>(GameEvents.OnMusicianArcCompleted, musician);
                
                Debug.Log("---> B");
                
                var id = GameState.Note.speaker.Id;
                Event.Call(GameEvents.OnDialogueTreeUpdate, id, emotion, selection, branches);
            }
            
            GameState.Note.speaker.RuntimeLink.PlayMelodyFor(emotion);
        }
    }
}