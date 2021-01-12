using System;
using Flux;
using UnityEngine;
using UnityEngine.UI;

namespace BeauTambour
{
    public class ImagePool : Pool<Image, PoolableImage>
    {
        #region Encapsulated Types

        [Serializable]
        public class ImageProvider : Provider<Image, PoolableImage> { }
        
        #endregion

        protected override Provider<Image, PoolableImage>[] Providers => providers;
        [SerializeField] private ImageProvider[] providers;
    }
}