using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private InputActionAsset navigationAsset;
        
        [Space, SerializeField] private float transitionTime;
        [SerializeField] private InputMapReference[] deactivableMaps;
        private bool[] mapStates;
        
        [Space, SerializeField] private AudioMixerSnapshot pauseSnapshot;
        [SerializeField] private AudioMixerSnapshot normalSnapshot;
    
        private bool state;
        private Coroutine pauseRoutine;
        
        private void Awake()
        {
            navigationAsset.Disable();
            
            state = false;
            mapStates = new bool[deactivableMaps.Length];

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
                phaseHandler.CurrentPhase.Pause();
                pauseSnapshot.TransitionTo(transitionTime);
                
                if (pauseRoutine != null) StopCoroutine(pauseRoutine);
                StartCoroutine(PauseRoutine(transitionTime, 0, true));
                
                Event.Call(GameEvents.OnGamePaused);
                state = true;
            }
            else
            {
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

            if (!state)
            {
                navigationAsset.Disable();
                for (var i = 0; i < deactivableMaps.Length; i++)
                {
                    if (mapStates[i] == true) deactivableMaps[i].Value.Enable();
                }
            }
            else
            {
                navigationAsset.Enable();
                for (var i = 0; i < deactivableMaps.Length; i++)
                {
                    var value = deactivableMaps[i].Value;
                    if (value.enabled == false) mapStates[i] = false;
                    else
                    {
                        value.Disable();
                        mapStates[i] = true;
                    }
                }
            }
            
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