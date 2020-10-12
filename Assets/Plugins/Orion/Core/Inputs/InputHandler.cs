using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orion
{
    public class InputHandler : SerializedMonoBehaviour
    {
        public InputActionAsset Source => inputSource;
        public IReadOnlyList<int> ActiveMaps => activeMaps;
        
        [SerializeField] private InputActionAsset inputSource;
        [SerializeField] private List<int> activeMaps = new List<int>();
        
        [ListDrawerSettings(CustomAddFunction = "AddInputActionLink")]
        [SerializeField] private List<InputActionLink> inputActionLinks = new List<InputActionLink>();
        
        private InputActionAsset runtimeInputSource;

        void OnEnable() => runtimeInputSource.Enable();
        void OnDisable() => runtimeInputSource.Disable();

        public void ActivateMap(string name) => runtimeInputSource.FindActionMap(name).Enable();
        public void DeactivateMap(string name) => runtimeInputSource.FindActionMap(name).Disable();
        
        protected virtual void Awake()
        {
            runtimeInputSource = Instantiate(inputSource);
            foreach (var activeMap in activeMaps) runtimeInputSource.actionMaps[activeMap].Enable();
            
            foreach (var inputActionLink in inputActionLinks) inputActionLink.TryBootUp(runtimeInputSource);
        }
        
        private InputActionLink AddInputActionLink() => new InputActionLink();
    }
}