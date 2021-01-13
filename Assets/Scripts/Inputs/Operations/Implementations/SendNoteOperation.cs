using System;
using System.Collections;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSendNoteOperation", menuName = "Beau Tambour/Operations/Send Note")]
    public class SendNoteOperation : PhaseBoundOperation
    {
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            Event.Register(GameEvents.OnNoteValidationDone, OnNoteValidationDone);
        }

        protected override void RelayedOnStart(EventArgs inArgs)
        {
            if (GameState.validationMade) return;
            
            GameState.validationMade = true;
            hook.StartCoroutine(ActivationRoutine());
        }

        private IEnumerator ActivationRoutine()
        {
            Event.Call(GameEvents.OnNoteValidation);
            
            var musicHandler = Repository.GetSingle<MusicHandler>(References.MusicHandler);
            musicHandler.Prepare();
            
            var time = 0.0f;
            var goal = 0.75f;

            while (time < goal)
            {
                musicHandler.Out(time / goal);
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            musicHandler.Out(1.0f);
            
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

        void OnNoteValidationDone() => hook.StartCoroutine(BringBackMusicRoutine());
        private IEnumerator BringBackMusicRoutine()
        {
            var musicHandler = Repository.GetSingle<MusicHandler>(References.MusicHandler);
            musicHandler.Prepare();
            
            var time = 0.0f;
            var goal = 1.25f;

            while (time < goal)
            {
                musicHandler.In(time / goal);
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            musicHandler.In(1.0f);
        }
    }
}