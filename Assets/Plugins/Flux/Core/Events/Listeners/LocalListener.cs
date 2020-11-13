using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Flux
{
    public abstract class LocalListener : MonoBehaviour
    {
        protected abstract string Address { get; }

        [SerializeField] protected Object key;
    }
    public abstract class LocalVoidListener : LocalListener
    {
        [SerializeField] private UnityEvent callback;

        void OnEnable() => Event.Register(Address, key, callback.Invoke);
        void OnDisable() => Event.Unregister(Address, key, callback.Invoke);
    }
    public abstract class LocalListener<T,TEvent> : LocalListener where TEvent : UnityEvent<T>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable() =>  Event.Register<T>(Address, key, callback.Invoke);
        void OnDisable() => Event.Unregister<T>(Address, key, callback.Invoke);
    }
    public abstract class LocalListener<T1,T2,TEvent> : LocalListener where TEvent : UnityEvent<T1,T2>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable() =>  Event.Register<T1,T2>(Address, key, callback.Invoke);
        void OnDisable() => Event.Unregister<T1,T2>(Address, key, callback.Invoke);
    }
    public abstract class LocalListener<T1,T2,T3,TEvent> : LocalListener where TEvent : UnityEvent<T1,T2,T3>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable() =>  Event.Register<T1,T2,T3>(Address, key, callback.Invoke);
        void OnDisable() => Event.Unregister<T1,T2,T3>(Address, key, callback.Invoke);
    }
    public abstract class LocalListener<T1,T2,T3,T4,TEvent> : LocalListener where TEvent : UnityEvent<T1,T2,T3,T4>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable() =>  Event.Register<T1,T2,T3,T4>(Address, key, callback.Invoke);
        void OnDisable() => Event.Unregister<T1,T2,T3,T4>(Address, key, callback.Invoke);
    }
}