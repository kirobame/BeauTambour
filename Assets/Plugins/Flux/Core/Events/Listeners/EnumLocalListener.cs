using UnityEngine;
using UnityEngine.Events;

namespace Flux
{
    public class EnumLocalListener<T,TEvent> : LocalListener<T,TEvent> where TEvent : UnityEvent<T>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
    public class EnumLocalListener<T1,T2,TEvent> : LocalListener<T1,T2,TEvent> where TEvent : UnityEvent<T1,T2>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
    public class EnumLocalListener<T1,T2,T3,TEvent> : LocalListener<T1,T2,T3,TEvent> where TEvent : UnityEvent<T1,T2,T3>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
    public class EnumLocalListener<T1,T2,T3,T4,TEvent> : LocalListener<T1,T2,T3,T4,TEvent> where TEvent : UnityEvent<T1,T2,T3,T4>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
}