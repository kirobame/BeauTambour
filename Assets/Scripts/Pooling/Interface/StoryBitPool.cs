using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class StoryBitPool : Pool<StoryBit, PoolableStoryBit>
    {
        #region Encapsulated Types

        [Serializable]
        public class StoryBitProvider : Provider<StoryBit, PoolableStoryBit> { }

        #endregion

        protected override Provider<StoryBit, PoolableStoryBit>[] Providers => providers;
        [SerializeField] private StoryBitProvider[] providers;
    }
}