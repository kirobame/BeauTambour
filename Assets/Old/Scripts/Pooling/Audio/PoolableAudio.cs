using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class PoolableAudio : Poolable<AudioSource>
    {
        public event Action OnDone;
        
        private Coroutine deactivationRoutine;
        
        void Update()
        {
            if (!Value.isPlaying && deactivationRoutine == null)
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