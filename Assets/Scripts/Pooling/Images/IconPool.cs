using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class IconPool : Pool<ProgressIcon, PoolableIcon>
    {
        #region Encapsulated Types

        [Serializable]
        public class IconProvider : Provider<ProgressIcon, PoolableIcon> { }
        
        #endregion

        protected override Provider<ProgressIcon, PoolableIcon>[] Providers => providers;
        [SerializeField] private IconProvider[] providers;
    }
}