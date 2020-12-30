using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class PoolableVisualEffectOld : Poolable<Animator>
    {
        public event Action OnDone;

        [SerializeField] private Vector2 offset;
        private Coroutine deactivationRoutine;

        void Update()
        {
            if (Value.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f && deactivationRoutine == null)
            {
                deactivationRoutine = StartCoroutine(DeactivationRoutine());
            }
        }

        public void ReceivedEndSignal() => OnDone?.Invoke();
        public void Place(Vector2 position) => transform.position = position + offset;
        
        private IEnumerator DeactivationRoutine()
        {
            OnDone?.Invoke();
            
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);

            deactivationRoutine = null;
        }
    }
}