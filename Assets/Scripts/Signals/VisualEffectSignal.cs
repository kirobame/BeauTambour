using System.Collections;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "newVfxSignal", menuName = "Beau Tambour/Signals/Vfx")]
    public class VisualEffectSignal : Signal
    {
        public override string Category => isBlurry ? "blurryvfx" : "vfx";

        [SerializeField] private bool isBlurry;
        [SerializeField] private EmotionEffect emotionEffectPrefab;
        
        private EmotionEffect playedEffect;

        public override void Execute(MonoBehaviour hook, ISpeaker speaker, string[] args)
        {
            Debug.Log($"[Vfx]:[{speaker.Actor}]:[{Key}]");
            
            var animationPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
            playedEffect = animationPool.RequestSinglePoolable(emotionEffectPrefab) as EmotionEffect;

            playedEffect.OnEnd += OnEffectEnd;
            playedEffect.Value.SetTrigger("Play");

            playedEffect.transform.parent = speaker.RuntimeLink.HeadSocket.Attach;
            playedEffect.transform.localPosition = Vector3.zero;
            playedEffect.transform.localScale = Vector3.one;
        }
        
        void OnEffectEnd()
        {
            playedEffect.OnEnd -= OnEffectEnd;
            End();
        }
    }
}