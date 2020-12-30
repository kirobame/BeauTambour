using System;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class VisualEffectPoolOld : Pool<Animator, PoolableVisualEffectOld>
    {
        #region Encapsulated Types

        [Serializable]
        public class VisualEffectProvider : Provider<Animator, PoolableVisualEffectOld> { }

        #endregion

        protected override Provider<Animator, PoolableVisualEffectOld>[] Providers => providers;

        [SerializeField] private VisualEffectProvider[] providers;
    }
}