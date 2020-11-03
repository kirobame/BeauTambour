using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class LineRendererPool : Pool<LineRenderer, PoolableLineRenderer>
    {
        #region Encapsulated Types

        [Serializable]
        public class LineRendererProvider : Provider<LineRenderer, PoolableLineRenderer> { }

        #endregion

        protected override Provider<LineRenderer, PoolableLineRenderer>[] Providers => providers;
        [SerializeField] private LineRendererProvider[] providers;
    }
}