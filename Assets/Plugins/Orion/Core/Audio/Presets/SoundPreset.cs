using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class SoundPreset : AudioPreset
    {
        [SerializeField, Range(0f, 1f)] private float volume = 1f; 
        [SerializeField, Range(0, 255)] private int priority = 128;
        [SerializeField, Range(-3f, 3f)] private float pitch = 1f;
        [SerializeField, Range(-1f, 1f)] private float panStereo = 0f;

        public override void Apply(AudioSource source)
        {
            source.volume = volume;
            source.priority = priority;
            source.pitch = pitch;
            source.panStereo = panStereo;
        }
    }
}