using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem.Users;

namespace Orion
{
    public abstract class Module : SerializedMonoBehaviour
    {
        public bool HasBinder { get; protected set; }
        protected IBinder Binder { get; private set; }

        private Coroutine coroutine;
        
        public virtual void Bind(IBinder binder)
        {
            if (HasBinder) return;

            Binder = binder;
            binder.onActionStarted += OnActionStarted;
            binder.onActionEnded += OnActionEnded;
        }
        public virtual void Unbind()
        {
            if (!HasBinder) return;

            Binder.onActionStarted -= OnActionStarted;
            Binder.onActionEnded -= OnActionEnded;
            Binder = null;
        }

        protected virtual void OnActionStarted(object input)
        {
            var routine = new WaitForEndOfFrame().ToRoutine();
            routine.Append(new IndefiniteRoutine()
            {
                action = () => OnAction(Binder.Input),
                waitInstruction = new WaitForEndOfFrame()
            });

            coroutine = StartCoroutine(routine.Call);
        }
        protected abstract void OnAction(object input);
        protected virtual void OnActionEnded(object input)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public abstract class Module<T> : Module
    {
        [FoldoutGroup("Events")] public OrionEvent<T> onActionStarted = new OrionEvent<T>();
        [FoldoutGroup("Events")] public OrionEvent<T> onAction = new OrionEvent<T>();
        [FoldoutGroup("Events")] public OrionEvent<T> onActionEnded = new OrionEvent<T>();
        
        protected T Input => (T)Binder.Input;
        private IBinder<T> CastedBinder => Binder as IBinder<T>;

        public override void Bind(IBinder binder)
        {
            var castedBinder = CastedBinder;
            if (castedBinder == null)
            {
                base.Bind(binder);
                return;
            }
            
            if (castedBinder.IsCompatibleWith(typeof(T))) base.Bind(binder);
        }

        protected override void OnActionStarted(object input)
        {
            var castedInput = (T)Convert.ChangeType(input, typeof(T));
            OnActionStarted(castedInput);
            
            base.OnActionStarted(input);
        }
        protected abstract void OnActionStarted(T input);
        
        protected override void OnAction(object input)
        {
            var castedInput = (T)Convert.ChangeType(input, typeof(T));
            OnAction(castedInput);
        }
        protected abstract void OnAction(T input);

        protected override void OnActionEnded(object input)
        {
            var castedInput = (T)Convert.ChangeType(input, typeof(T));
            OnActionEnded(castedInput);
            
            base.OnActionEnded(input);
        }
        protected abstract void OnActionEnded(T input);
    }
}