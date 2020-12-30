using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class PoolableAnimation : Poolable<Animator>
    {
        [SerializeField] private string startTag;
        [SerializeField] private string resetTag;
        
        private bool hasBeenIn;

        void Update()
        {
            var state = Value.GetCurrentAnimatorStateInfo(0);

            if (!hasBeenIn)
            {
                if (state.IsTag(startTag)) hasBeenIn = true;
            }
            else if (state.IsTag(resetTag))
            {
                hasBeenIn = false;
                gameObject.SetActive(false);
            }
        }
    }
}