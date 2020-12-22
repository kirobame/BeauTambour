using UnityEngine;
using Event = Flux.Event;

namespace Deprecated
{
    public class Bootstrapper : MonoBehaviour
    {
        #region Encapsulated Types

        public enum EventType
        {
            OnBootup,
        }
        #endregion

        [SerializeField] private ScriptableObject[] assets;

        void Awake()
        {
            Event.Open(EventType.OnBootup);
            foreach (var asset in assets)
            {
                if (!(asset is IBootable bootable)) return;
                bootable.BootUp();
            }
        }

        void Start() => Event.Call(EventType.OnBootup);
    }
}