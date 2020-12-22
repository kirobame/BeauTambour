using System;
using UnityEngine;

namespace Deprecated
{
    [Serializable]
    public struct PhaseCallbackAddress
    {
        public PhaseCallbackAddress(PhaseType type, PhaseCallback callback)
        {
            this.type = type;
            this.callback = callback;
        }
        
        [SerializeField] private PhaseType type;
        [SerializeField] private PhaseCallback callback;

        public string Get() => $"{type}.{callback}";
    }
}