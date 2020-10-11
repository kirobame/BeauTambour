using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orion
{
    [HideReferenceObjectPicker]
    public class InputActionLink
    {
        #if UNITY_EDITOR

        private bool CanModuleBeEnabled() => reference != null;
        private void OnModuleChanged()
        {
            if (module == null) return;
            var inputType = module.GetType().BaseType.GetGenericArguments().First();
            
            var binderType = typeof(Binder<>).MakeGenericType(inputType);
            binder = Activator.CreateInstance(binderType) as Binder;
        }
        
        #endif
        
        public Binder Binder => binder;
        public Module Module => module;
        
        public InputAction RuntimeInputAction { get; private set; }
        
        [SerializeField] private InputActionReference reference;
        
        [EnableIf("CanModuleBeEnabled"), OnValueChanged("OnModuleChanged")]
        [SerializeField] private Module module;
        [HideInInspector, SerializeField] private Binder binder;

        public bool TryBootUp(InputActionAsset runtimeAsset)
        {
            try
            {
                RuntimeInputAction = runtimeAsset[reference.name]; ;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                return false;
            }
            
            RuntimeInputAction.started += OnInputActionStarted;
            RuntimeInputAction.performed += OnInputAction;
            RuntimeInputAction.canceled += OnInputActionEnded;

            module.Bind(binder);
            
            return true;
        }
        
        private void OnInputActionStarted(InputAction.CallbackContext context)
        {
            binder.Update(context.ReadValueAsObject());
            binder.Start();
        }
        private void OnInputAction(InputAction.CallbackContext context) => binder.Update(context.ReadValueAsObject());
        private void OnInputActionEnded(InputAction.CallbackContext context)
        {
            binder.Update(context.ReadValueAsObject());
            binder.End();
        }
    }
}