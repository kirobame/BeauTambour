using System;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class VisualEffectPool : Pool<Animator, PoolableVisualEffect>
    {
        #region Encapsulated Types

        [Serializable]
        public class VisualEffectProvider : Provider<Animator, PoolableVisualEffect> { }

        #endregion

        protected override Provider<Animator, PoolableVisualEffect>[] Providers => providers;

        [SerializeField] private VisualEffectProvider[] providers;
    }
}