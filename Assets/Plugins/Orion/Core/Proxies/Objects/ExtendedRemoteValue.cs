using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class ExtendedRemoteValue<T> : RemoteValue<T>
    {
        [SerializeField] private bool hasValueChangeCallback;
        [SerializeField, ShowIf("hasValueChangeCallback")] private RemoteEvent<T> onValueChanged;
        
        [Space, SerializeField] private bool hasHistoryChangeCallback;
        [SerializeField, ShowIf("hasHistoryChangeCallback")] private RemoteEvent<T,T> onHistoryChanged;

        public override void Write(T value)
        {
            var previousValue = this.value;
            base.Write(value);
            
            if (hasHistoryChangeCallback) onHistoryChanged.Invoke(previousValue, this.value);
            if (hasValueChangeCallback) onValueChanged.Invoke(this.value);
        }
    }
}