using System;
using System.Collections;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerIntermediary : MonoBehaviour
    {
        public Animator Animator => animator;
        
        [Space, SerializeField] private Animator animator;
        [SerializeField] private EmotionMelodyRegistry emotionMelodyRegistry;
        [SerializeField] private EmotionEffectRegistry emotionEffectRegistry;

        private RuntimeCharacter source;
        
        private PoolableAudio poolableAudio;
        private PoolableAnimation poolableAnimation;

        public void SetSource(RuntimeCharacter source) => this.source = source; 
        
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

            if (emotionEffectRegistry.TryGet(emotion, out var effectPrefab))
            {
                var animationPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
                poolableAnimation = animationPool.RequestSinglePoolable(effectPrefab);

                poolableAnimation.transform.parent = source.HeadSocket.Attach;
                poolableAnimation.transform.localPosition = Vector3.zero;
                poolableAnimation.transform.localScale = Vector3.one;
                
                poolableAnimation.Value.SetTrigger("In");
            }
        }

        public void ActOut(Emotion emotion) => animator.SetTrigger(emotion.ToString());

        void OnMelodyEnd()
        {
            poolableAudio.OnDone -= OnMelodyEnd;
            StartCoroutine(MelodyTerminationRoutine());
        }
        private IEnumerator MelodyTerminationRoutine()
        {
            if (poolableAnimation != null) poolableAnimation.Value.SetTrigger("Out");
            animator.SetBool("IsPlaying", false);
            
            yield return new WaitForSeconds(0.75f);
            
            Event.Call(GameEvents.OnNoteValidationDone);
        }
    }
}