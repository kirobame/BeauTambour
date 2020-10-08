using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Orion
{
    public class OutputPreset : AudioPreset
    {
        [SerializeField] private AudioMixerGroup group;
        [SerializeField] private bool mute;
        [SerializeField] private bool loop;
        
        public override void Apply(AudioSource source)
        {
            source.outputAudioMixerGroup = group;
            source.mute = mute;
            source.loop = loop;
        }
    }
}