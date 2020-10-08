using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class AudioPoolable : Poolable<AudioSource>
    {
        private void OnEnable()
        {
            if (Value.loop) return;
            
            var routine = new WaitForEndOfFrame().ToRoutine();
            routine.Append(new ActionRoutine() {action = SetupLifetime});
            StartCoroutine(routine.Call);
        }

        private void SetupLifetime()
        {
            var routine = new WaitForSeconds(Value.clip.length + 0.5f).ToRoutine();
            routine.Append(new ActionRoutine() {action = () => gameObject.SetActive(false)});
            StartCoroutine(routine.Call);
        }

        [Button]
        private void Play() => Value.Play();
    }
}