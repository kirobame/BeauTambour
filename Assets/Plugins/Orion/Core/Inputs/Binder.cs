using System;

namespace Orion
{
    public class Binder : IBinder
    {
        #region Events

        event Action<object> IBinder.onActionStarted
        {
            add => onActionStarted += value;
            remove => onActionStarted -= value;
        }
        private event Action<object> onActionStarted;

        event Action<object> IBinder.onActionEnded
        {
            add => onActionEnded += value;
            remove => onActionEnded -= value;
        }
        private event Action<object> onActionEnded;

        #endregion
        
        public object Input { get; private set; }
        
        public virtual void Start() => onActionStarted?.Invoke(Input);
        public virtual void Update(object input) => Input = input; 
        public virtual void End() => onActionEnded.Invoke(Input);
    }
    
    public class Binder<T> : Binder, IBinder<T>
    {
        public T CastedInput => (T)Input;

        public bool IsCompatibleWith(Type type) => type == typeof(T);
    }
}