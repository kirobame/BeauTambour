using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class PoolableAudio : Poolable<AudioSource>
    {
        void Update()
        {
            if (!Value.isPlaying) gameObject.SetActive(false);
        }
    }
}