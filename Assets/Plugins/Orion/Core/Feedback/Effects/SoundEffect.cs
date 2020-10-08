using System.Collections;
using UnityEngine;

namespace Orion
{
    public class SoundEffect : Feedback
    {
        [SerializeField] private IReadable<AudioSource> audioSourceProxy;
        private AudioSource audioSource => audioSourceProxy.Read();
        
        [SerializeField] private AudioBundle audioBundle;
        
        public override IEnumerator GetRoutine()
        {
            audioBundle.AssignTo(audioSource);
            audioSource.Play();

            yield break;
        }
    }
}