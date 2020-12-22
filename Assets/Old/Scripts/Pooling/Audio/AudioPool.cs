using System;
using Flux;
using UnityEngine;

namespace Deprecated
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