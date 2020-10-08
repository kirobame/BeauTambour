using System.Collections;
using UnityEngine;

namespace Orion
{
    public class PooledSoundEffect : Feedback
    {
        [SerializeField] private IReadable<IProvider<AudioSource>> audioSourceProvider;
        [SerializeField] private AudioBundle audioBundle;

        public override IEnumerator GetRoutine()
        {
            var audioSource = audioSourceProvider.Read().GetInstance();
            audioBundle.AssignTo(audioSource);
            
            audioSource.Play();
            
            yield break;
        }
    }
}