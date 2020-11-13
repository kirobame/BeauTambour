using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Beau Tambour/Audio")]
    public class AudioEffect : Effect
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volume;
        [SerializeField] private float pitch;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
            var audioSource = audioPool.RequestSingle();

            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.clip = clip;
            
            audioSource.Play();
            
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}