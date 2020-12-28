using System;
using UnityEngine;
using UnityEngine.Audio;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSingleAudioPackage", menuName = "Beau Tambour/General/Packages/Single Audio")]
    public class SingleAudioPackage : AudioPackage
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField, Range(0, 1)] private float volume = 1;
        [SerializeField, Range(-3, 3)] private float pitch = 1;
        
        public override void AssignTo(AudioSource source, EventArgs inArgs)
        {
            source.clip = clip;
            source.outputAudioMixerGroup = mixerGroup;

            source.volume = volume;
            source.pitch = pitch;
        }
    }
}