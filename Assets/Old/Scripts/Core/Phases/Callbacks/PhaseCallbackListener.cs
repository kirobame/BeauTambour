using Flux;
using UnityEngine;
using UnityEngine.Events;
using Event = Flux.Event;

namespace Deprecated
{
    [IconProxy(typeof(Listener))]
    public class PhaseCallbackListener : MonoBehaviour
    {
        [SerializeField] private PhaseCallbackAddress address;
        [SerializeField] private UnityEvent callback;

        void OnEnable() => Event.Register(address.Get(), callback.Invoke);
        void OnDisable() => Event.Unregister(address.Get(), callback.Invoke);
    }
}