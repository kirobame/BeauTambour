using System;
using UnityEngine;

namespace Flux
{
    public abstract class Operation : ScriptableObject, IBindable
    {
        public event Action<EventArgs> onStart;
        public event Action<EventArgs> onUpdate;
        public event Action<EventArgs> onEnd;
        
        protected MonoBehaviour hook { get; private set; }
        protected bool isBinded;

        public virtual void Initialize(MonoBehaviour hook) => this.hook = hook;
        protected virtual void OnDestroy() => Unbind();
        
        public abstract void Bind(IBindable bindable);
        public abstract void Unbind();
        
        protected void Begin(EventArgs outArgs) => onStart?.Invoke(outArgs);
        protected void Prolong(EventArgs outArgs) => onUpdate?.Invoke(outArgs);
        protected void End(EventArgs outArgs) => onEnd?.Invoke(outArgs);
    }
}