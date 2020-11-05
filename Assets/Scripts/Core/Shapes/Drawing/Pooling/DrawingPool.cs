using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class DrawingPool : Pool<LineRenderer, PoolableDrawing>
    {
        #region Encapsulated Types

        [Serializable]
        public class LineRendererProvider : Provider<LineRenderer, PoolableDrawing> { }

        #endregion

        protected override Provider<LineRenderer, PoolableDrawing>[] Providers => providers;
        [SerializeField] private LineRendererProvider[] providers;
    }
}