using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewReactSignal", menuName = "Beau Tambour/Signals/React")]
    public class ReactSignal : Signal
    {
        public override string Category => isBlurry ? "blurryreact" : "react";
        
        [SerializeField] private bool isBlurry;
        
        [Space, SerializeField] private VisualEffectSignal visualEffectSignal;
        [SerializeField] private AnimationSignal animationSignal;
        
        private int count;
        
        public override void Execute(MonoBehaviour hook, Character speaker, string[] args)
        {
            count = 0;

            visualEffectSignal.OnEnd += OnSignalDone;
            animationSignal.OnEnd += OnSignalDone;
            
            visualEffectSignal.Execute(hook, speaker, args);
            animationSignal.Execute(hook, speaker, args);
        }

        void OnSignalDone()
        {
            count++;
            if (count == 2)
            {
                visualEffectSignal.OnEnd -= OnSignalDone;
                animationSignal.OnEnd -= OnSignalDone;
                
                End();
            }
        }
    }
}