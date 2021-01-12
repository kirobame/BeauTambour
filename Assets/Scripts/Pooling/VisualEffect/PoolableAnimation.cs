using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class PoolableAnimation : Poolable<Animator>
    {
        public Action OnEnd;
        
        [SerializeField] private string startTag;
        [SerializeField] private string resetTag;
        
        private bool hasBeenIn;
        
        void Update()
        {
            if (!hasBeenBootedUp || !Value.enabled) return;
            
            var state = Value.GetCurrentAnimatorStateInfo(0);
            if (!hasBeenIn)
            {
                if (state.IsTag(startTag)) hasBeenIn = true;
            }
            else if (state.IsTag(resetTag))
            {
                OnEnd?.Invoke();
                
                hasBeenIn = false;
                gameObject.SetActive(false);
            }
        }
    }
}