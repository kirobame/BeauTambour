using System.Collections;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewVfxSignal", menuName = "Beau Tambour/Signals/Vfx")]
    public class VisualEffectSignal : Signal
    {
        public override string Category => isBlurry ? "blurryvfx" : "vfx";

        [SerializeField] private bool isBlurry;
        [SerializeField] private EmotionEffect emotionEffectPrefab;

        [Space, SerializeField] private int frameFreeze;
        [SerializeField] private AnimationCurve loopCurve;
        [SerializeField] private float loopLength;
        [SerializeField] private int cycles;
        
        private EmotionEffect playedEffect;

        public override void Execute(MonoBehaviour hook, Character speaker, string[] args)
        {
            //Debug.Log($"{Time.time} -- [Vfx]:[{speaker.Actor}]:[{Key}]");
            
            var animationPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
            playedEffect = animationPool.RequestSinglePoolable(emotionEffectPrefab) as EmotionEffect;
            
            playedEffect.OnEnd += OnEffectEnd;
            playedEffect.Value.SetTrigger("Play");

            playedEffect.transform.parent = speaker.RuntimeLink.HeadSocket.Value;
            playedEffect.transform.localPosition = Vector3.zero;
            playedEffect.transform.localScale = Vector3.one;

            if (!isBlurry) hook.StartCoroutine(FreezeRoutine());
        }
        private IEnumerator FreezeRoutine()
        {
            var animator = playedEffect.Value;
            var state = animator.GetCurrentAnimatorStateInfo(0);
            
            while (!state.IsTag("In"))
            {
                yield return new WaitForEndOfFrame();
                state = animator.GetCurrentAnimatorStateInfo(0);
            }
            
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
            var secondPerFrame = 1.0f / clipInfo.clip.frameRate;
            var freezeTime = frameFreeze * secondPerFrame;

            var time = clipInfo.clip.length * state.normalizedTime;
            while (time < freezeTime)
            {
                yield return new WaitForEndOfFrame();
                
                state = animator.GetCurrentAnimatorStateInfo(0);
                time = clipInfo.clip.length * state.normalizedTime;
            }
            animator.enabled = false;

            for (var i = 0; i < cycles; i++)
            {
                var loopTime = 0.0f;
                while (loopTime < loopLength)
                {
                    Scale(loopTime / loopLength);   
                    
                    yield return new WaitForEndOfFrame();
                    loopTime += Time.deltaTime;
                }
                Scale(1.0f);
                
                void Scale(float ratio)
                {
                    var scale = loopCurve.Evaluate(ratio);
                    animator.transform.localScale = Vector3.one * (1.0f + scale);
                }
            }
            animator.enabled = true;
        }
        
        void OnEffectEnd()
        {
            playedEffect.OnEnd -= OnEffectEnd;
            End();
        }
    }
}