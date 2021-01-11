using System;
using System.Collections;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class RuntimeCharacter : RuntimeCharacterBase
    {
        public override Animator Animator => animator;
        [SerializeField] private Animator animator;
        
        public override Character Asset => asset;
        [SerializeField] private Character asset;

        public override Vector2 DialogueAnchor => dialogueAnchor.position;
        [Space,SerializeField] private Transform dialogueAnchor;

        public override Vector2 SelectionAnchor => selectionAnchor.position;
        [SerializeField] private Transform selectionAnchor;
        
        public override Attach HeadSocket => headSocket;
        [SerializeField] private Attach headSocket;
        
        [Space, SerializeField] private EmotionMelodyRegistry emotionMelodyRegistry;
        [SerializeField] private EmotionEffectRegistry emotionEffectRegistry;
        
        private PoolableAudio poolableAudio;
        private PoolableAnimation poolableAnimation;

        protected virtual void Start() => asset.Bootup(this);
        public override void Reboot() => dialogueAnchor.localPosition = Vector3.zero;

        public override bool ActOut(Emotion emotion)
        {
            animator.SetTrigger(emotion.ToString());
            return true;
        }
        public override bool PlayMelodyFor(Emotion emotion)
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

                poolableAnimation.transform.parent = headSocket.Value;
                poolableAnimation.transform.localPosition = Vector3.zero;
                poolableAnimation.transform.localScale = Vector3.one;
                
                poolableAnimation.Value.SetTrigger("In");
            }

            return true;
        }

        public override bool BeginTalking()
        {
            animator.SetBool("IsTalking", true);
            return true;
        }
        public override void StopTalking() => animator.SetBool("IsTalking", false);
        
        void OnMelodyEnd()
        {
            poolableAudio.OnDone -= OnMelodyEnd;
            StartCoroutine(MelodyTerminationRoutine());
        }
        private IEnumerator MelodyTerminationRoutine()
        {
            poolableAnimation.Value.SetTrigger("Out");
            animator.SetBool("IsPlaying", false);
            
            yield return new WaitForSeconds(0.75f);
            
            Event.Call(GameEvents.OnNoteValidationDone);
        }
    }
}