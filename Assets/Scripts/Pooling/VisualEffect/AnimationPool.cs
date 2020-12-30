using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class AnimationPool : Pool<Animator, PoolableAnimation>
    {
        #region Encapsulated Types

        [Serializable]
        public class AnimationProvider : Provider<Animator, PoolableAnimation> { }

        #endregion

        protected override Provider<Animator, PoolableAnimation>[] Providers => providers;
        [SerializeField] private AnimationProvider[] providers;
    }
}