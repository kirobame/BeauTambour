using System;
using BeauTambour;
using Flux;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewPlayIndexedAudioOperation", menuName = "Beau Tambour/Operations/Play Indexed Audio")]
    public class PlayIndexedAudioOperation : SingleOperation
    {
        [SerializeField, Range(0,1)] private float volume = 1;
        [SerializeField] private AudioClip[] clips;

        public override void OnStart(EventArgs inArgs)
        {
            var index = -1;
            
            if (inArgs is SingleEventArgs<int> indexArgs) index = indexArgs.Value;
            else if (inArgs is Vector2EventArgs axisArgs)
            {
                if (axisArgs.value.x == -1) index = 0;
                else if (axisArgs.value.x == 1) index = 1;
                else if (axisArgs.value.y == 1) index = 2;
                else if (axisArgs.value.y == -1) index = 3;
            }

            if (index == -1) return;
            
            var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
            var audioSource = audioPool.RequestSingle();

            audioSource.volume = volume;
            audioSource.clip = clips[index];
                
            audioSource.Play();
        }
    }
}