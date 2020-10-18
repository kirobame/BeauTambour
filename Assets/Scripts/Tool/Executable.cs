using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Tooling
{
    public abstract class Executable : SerializedMonoBehaviour, IBindable
    {
        private Action<InputAction.CallbackContext> action;
         
        protected virtual void Awake()
        {
            Debug.Log($"Initializing {name}");
            action = ctxt => Execute();
        }

        public abstract void Execute();
        
        void IBindable.Bind(InputAction inputAction) => inputAction.performed += action;
        void IBindable.Unbind(InputAction inputAction)=> inputAction.performed -= action;
    }
}