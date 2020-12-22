using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Event = Flux.Event;

namespace Deprecated
{
    public class DeviceAlternationHandler : MonoBehaviour
    {
        #region Encapsulated Types

        public enum EventType
        {
            OnChange,
        }

        #endregion

        public static bool IsMouse => isMouse;
        private static bool isMouse;
        
        [SerializeField] private InputActionReference[] mouseInputs;
        [SerializeField] private InputActionReference[] gamepadInputs;
        
        private InputDevice mouseDevice;
        private InputDevice gamepadDevice;
        
        void Awake()
        {
            isMouse = true;
            
            gamepadDevice = InputSystem.GetDevice("XInputControllerWindows");
            if (gamepadDevice == null)
            {
                Debug.Log("No need for device alternation : Shutting down");
                gameObject.SetActive(false);

                return;
            }
            mouseDevice = InputSystem.GetDevice("Mouse");
            
            foreach (var gamepadInput in gamepadInputs) gamepadInput.action.Disable();
            Event.Open(EventType.OnChange);
        }

        void Update()
        {
            if (isMouse && gamepadDevice.lastUpdateTime > mouseDevice.lastUpdateTime)
            {
                isMouse = false;
                
                foreach (var gamepadInput in gamepadInputs) gamepadInput.action.Enable();
                foreach (var mouseInput in mouseInputs) mouseInput.action.Disable();

                Event.Call(EventType.OnChange);
            }
            else if (!isMouse && mouseDevice.lastUpdateTime > gamepadDevice.lastUpdateTime)
            {
                isMouse = true;
                
                foreach (var gamepadInput in gamepadInputs) gamepadInput.action.Disable();
                foreach (var mouseInput in mouseInputs) mouseInput.action.Enable();
                
                Event.Call(EventType.OnChange);
            }
        }
    }
}