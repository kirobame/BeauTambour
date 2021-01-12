﻿using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewReactSignal", menuName = "Beau Tambour/Signals/React")]
    public class ReactSignal : Signal
    {
        [SerializeField] private VisualEffectSignal visualEffectSignal;
        [SerializeField] private AnimationSignal animationSignal;
        
        public override string Category => "react";
        
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