using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
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

        [SerializeField] private InputMapReference[] mapReferences;

        void Awake()
        {
            Event.Open(EventType.OnPause);
            Event.Open(EventType.OnUnpause);

            Event.Register(EventType.OnPause, OnPause);
            Event.Register(EventType.OnUnpause, OnUnpause);
        }

        public void Pause() => Event.Call(EventType.OnPause);
        public void Unpause()
        {
            Debug.Log("Test");
            Event.Call(EventType.OnUnpause);
        }

        void OnPause()
        {
            Time.timeScale = 0;
            foreach (var mapReference in mapReferences) mapReference.Value.Disable();
        }
        void OnUnpause()
        {
            Time.timeScale = 1;
            foreach (var mapReference in mapReferences) mapReference.Value.Enable();
        }

        public void Quit() => Application.Quit();
    }
}