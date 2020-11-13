using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class PoolableAudio : Poolable<AudioSource>
    {
        private Coroutine deactivationRoutine;
        
        void Update()
        {
            if (!Value.isPlaying && deactivationRoutine == null) deactivationRoutine = StartCoroutine(DeactivationRoutine());
        }

        private IEnumerator DeactivationRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);

            deactivationRoutine = null;
        }
    }
}