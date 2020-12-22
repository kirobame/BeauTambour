using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace Deprecated
{
    public class PauseMenu : MonoBehaviour
    {
        #region Encapsulated Types

        [EnumAddress]
        public enum EventType
        {
            OnPause,
            OnUnpause
        }
        #endregion

        [SerializeField] private InputMapReference gameplayMap;
        [SerializeField] private InputMapReference outcomeMap;
        private bool isPaused;

        void Awake()
        {
            isPaused = false;
            
            Event.Open(EventType.OnPause);
            Event.Open(EventType.OnUnpause);

            Event.Register(EventType.OnPause, OnPause);
            Event.Register(EventType.OnUnpause, OnUnpause);
        }

        public void Pause()
        {
            if (isPaused) return;
            Event.Call(EventType.OnPause);
        }

        public void Unpause()
        {
            if (!isPaused) return;
            Event.Call(EventType.OnUnpause);
        }

        void OnPause()
        {
            Time.timeScale = 0;

            gameplayMap.Value.Disable();
            outcomeMap.Value.Disable();
            
            isPaused = true;
        }
        void OnUnpause()
        {
            Time.timeScale = 1;
            
            var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
            if (phaseHandler.CurrentType == PhaseType.Outcome) outcomeMap.Value.Enable();
            else gameplayMap.Value.Enable();
            
            isPaused = false;
        }

        public void Quit() => Application.Quit();
    }
}