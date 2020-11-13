using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Bootstrapper : MonoBehaviour
    {
        #region Encapsulated Types

        public enum EventType
        {
            OnBootup,
        }
        #endregion

        void Awake() => Event.Open(EventType.OnBootup);
        void Start() => Event.Call(EventType.OnBootup);
    }
}