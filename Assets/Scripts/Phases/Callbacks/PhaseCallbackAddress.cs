using System;
using UnityEngine;

namespace BeauTambour
{
    [Serializable]
    public struct PhaseCallbackAddress
    {
        public PhaseCallbackAddress(PhaseCategory category, PhaseCallback callback)
        {
            this.category = category;
            this.callback = callback;
        }
        
        [SerializeField] private PhaseCategory category;
        [SerializeField] private PhaseCallback callback;

        public string Get() => $"{category}.{callback}";
    }
}