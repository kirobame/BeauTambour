using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using UnityEngine.Audio;
using Event = Flux.Event;

namespace BeauTambour
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private float transitionTime;
        [SerializeField] private InputMapReference[] deactivableMaps;
        
        [Space, SerializeField] private AudioMixerSnapshot pauseSnapshot;
        [SerializeField] private AudioMixerSnapshot normalSnapshot;
    
        private bool state;
        private Coroutine pauseRoutine;
        
        private void Awake()
        {
            state = false;
            
            Repository.Reference(this, References.PauseMenu);
            
            Event.Open(GameEvents.OnGamePaused);
            Event.Open(GameEvents.OnGameResumed);
            
            Event.Open(GameEvents.OnPauseToggled);
            Event.Register(GameEvents.OnPauseToggled, OnPauseToggled);
        }

        public void Quit() => Application.Quit();
        
        void OnPauseToggled()
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);

            if (!state)
            {
                foreach (var mapReference in deactivableMaps) mapReference.Value.Disable();

                phaseHandler.CurrentPhase.Pause();
                pauseSnapshot.TransitionTo(transitionTime);
                
                if (pauseRoutine != null) StopCoroutine(pauseRoutine);
                StartCoroutine(PauseRoutine(transitionTime, 0, true));
                
                Event.Call(GameEvents.OnGamePaused);
                state = true;
            }
            else
            {
                foreach (var mapReference in deactivableMaps) mapReference.Value.Enable();
                
                phaseHandler.CurrentPhase.Resume();
                normalSnapshot.TransitionTo(transitionTime);
                
                if (pauseRoutine != null) StopCoroutine(pauseRoutine);
                StartCoroutine(PauseRoutine(transitionTime, 1, false));
                
                Event.Call(GameEvents.OnGameResumed);
                state = false;
            }
        }

        private IEnumerator PauseRoutine(float duration, float goal, bool state)
        {
            var initialScale = Time.timeScale;
            var time = 0f;
            
            while (time < duration)
            {
                Time.timeScale = Mathf.Lerp(initialScale, goal, time / duration);
                
                time += Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            ToggleAudio(state);

            Time.timeScale = goal;
            pauseRoutine = null;
        }

        private void ToggleAudio(bool state)
        {
            var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
            foreach (var poolableAudio in audioPool.UsedInstances)
            {
                if (poolableAudio.group != Group.Scaled) continue;
                
                if (state) poolableAudio.Value.Pause();
                else poolableAudio.Value.UnPause();
            }
        }
    }
}