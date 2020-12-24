using System;
using Deprecated;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class AudioPool : Pool<AudioSource, PoolableAudio>
    {
        #region Encapasulated Types

        [Serializable]
        public class AudioProvider : Provider<AudioSource, PoolableAudio> { }

        #endregion

        protected override Provider<AudioSource, PoolableAudio>[] Providers => providers;
        [SerializeField] private AudioProvider[] providers;
    }
}