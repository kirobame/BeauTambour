using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux
{
    public abstract class InputHandler : ScriptableObject, IBindable
    {
        public event Action<EventArgs> onStart;
        public event Action<EventArgs> onUpdate;
        public event Action<EventArgs> onEnd;
        
        protected InputAction bindedAction;
        protected MonoBehaviour hook;

        public virtual void Initialize(MonoBehaviour hook) => this.hook = hook;
        protected virtual void OnDestroy() => Unbind();
        
        public virtual void Bind(InputAction inputAction)
        {
            if (bindedAction != null) Unbind();
            bindedAction = inputAction;
        }
        public abstract void Unbind();
        
        protected void Begin(EventArgs args) => onStart?.Invoke(args);
        protected void Prolong(EventArgs args) => onUpdate?.Invoke(args);
        protected void End(EventArgs args) => onEnd?.Invoke(args);

        public abstract bool OnStarted();
        public abstract bool OnPerformed();
        public abstract bool OnCanceled();
    }
    public abstract class InputHandler<TInput> : InputHandler
    {
        #region Encapsulated Types

        protected enum Phase
        {
            Started,
            Performed,
            Canceled
        }
        #endregion
        
        private ActionCallbacks<InputAction.CallbackContext> inputCallbacks;
        protected Phase phase = Phase.Canceled;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            inputCallbacks = new ActionCallbacks<InputAction.CallbackContext>()
            {
                onStart = ctxt => OnStarted((TInput) Convert.ChangeType(ctxt.ReadValueAsObject(), typeof(TInput))),
                onUpdate = ctxt => OnPerformed((TInput) Convert.ChangeType(ctxt.ReadValueAsObject(), typeof(TInput))),
                onEnd = ctxt => OnCanceled((TInput) Convert.ChangeType(ctxt.ReadValueAsObject(), typeof(TInput)))
            };
        }

        public override void Bind(InputAction inputAction)
        {
            base.Bind(inputAction);
            
            inputAction.started += inputCallbacks.onStart;
            inputAction.performed += inputCallbacks.onUpdate;
            inputAction.canceled += inputCallbacks.onEnd;
        }
        public override void Unbind()
        {
            bindedAction.started -= inputCallbacks.onStart;
            bindedAction.performed -= inputCallbacks.onUpdate;
            bindedAction.canceled -= inputCallbacks.onEnd;
            
            bindedAction = null;
        }

        public override bool OnStarted() => OnStarted(default(TInput));
        public virtual bool OnStarted(TInput input)
        {
            if (phase != Phase.Canceled) return false;
            
            phase = Phase.Started;
            return true;
        }
        
        public override bool OnPerformed() => OnPerformed(default(TInput));
        public virtual bool OnPerformed(TInput input)
        {
            if (phase != Phase.Started && phase != Phase.Performed) return false;
            
            phase = Phase.Performed;
            return true;
        }
        
        public override bool OnCanceled() => OnCanceled(default(TInput));
        public virtual bool OnCanceled(TInput input)
        {
            if (phase != Phase.Performed) return false;

            phase = Phase.Canceled;
            return true;
        }
    }
}