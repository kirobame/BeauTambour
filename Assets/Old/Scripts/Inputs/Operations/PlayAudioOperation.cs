using System;
using BeauTambour;
using Flux;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewPlayAudioOperation", menuName = "Beau Tambour/Operations/Play Audio")]
    public class PlayAudioOperation : SingleOperation
    {
        [SerializeField, Range(0,1)] private float volume = 1;
        [SerializeField, Range(-3,3)] private float pitch = 1;
        [SerializeField] private AudioClip clip;
        
        public override void OnStart(EventArgs inArgs)
        {
            var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
            var audioSource = audioPool.RequestSingle();

            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.clip = clip;
                
            audioSource.Play();
        }
    }
}