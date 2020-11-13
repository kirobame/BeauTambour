using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux
{
    [Serializable]
    public class InputMapReference
    {
        public InputActionMap Value => asset.FindActionMap(name);
        public string Name => name;
        
        [SerializeField] private InputActionAsset asset;
        [SerializeField] private string name;
    }
}