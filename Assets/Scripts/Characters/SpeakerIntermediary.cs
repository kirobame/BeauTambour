using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerIntermediary : MonoBehaviour
    {
        [Space, SerializeField] private Animator animator;
        [SerializeField] private EmotionMelodyRegistry emotionMelodyRegistry;

        private PoolableAudio poolableAudio;

        public void BeginTalking() => animator.SetBool("IsTalking", true);
        public void StopTalking() => animator.SetBool("IsTalking", false);

        public void PlayMelodyFor(Emotion emotion)
        {
            animator.SetBool("IsPlaying", true);
            
            var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
            poolableAudio = audioPool.RequestSinglePoolable();

            var melody = emotionMelodyRegistry[emotion];
            melody.AssignTo(poolableAudio.Value, EventArgs.Empty);

            poolableAudio.OnDone += OnMelodyEnd;
            poolableAudio.Value.Play();
        }

        void OnMelodyEnd()
        {
            poolableAudio.OnDone -= OnMelodyEnd;
            
            Event.Call(GameEvents.OnNoteValidationDone);
            animator.SetBool("IsPlaying", false);
        }
    }
}