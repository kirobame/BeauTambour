using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class PoolableAudio : Poolable<AudioSource>
    {
        public event Action OnDone;

        public Group group;
        
        private Coroutine deactivationRoutine;

        public override void Prepare() => group = Group.Scaled;

        void Update()
        {
            if (!hasBeenBootedUp || Value.loop) return;

            if (Value.clip.length - Value.time <= 0.1f)
            {
                OnDone?.Invoke();
                deactivationRoutine = StartCoroutine(DeactivationRoutine());
            }
        }

        private IEnumerator DeactivationRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);

            deactivationRoutine = null;
        }
    }
}