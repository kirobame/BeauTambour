using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class ReferencerBase : MonoBehaviour
    {
        [SerializeField] private bool lifetimeLinkage;
        
        public abstract object Content { get; }
        
        void OnEnable()
        {
            if (!lifetimeLinkage) return;
            Register();
        }
        void OnDisable()
        {
            if (!lifetimeLinkage) return;
            Unregister();
        }

        void Awake()
        {
            if (lifetimeLinkage) return;
            Register();
        }
        void OnDestroy()
        {
            if (lifetimeLinkage) return;
            Unregister();
        }

        protected abstract void Register();
        protected abstract void Unregister();
    }
}