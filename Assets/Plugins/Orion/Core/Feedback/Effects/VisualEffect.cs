using System.Collections;
using UnityEngine;

namespace Orion
{
    public class VisualEffect : Feedback
    {
        [SerializeField] private IReadable<ParticleSystem> particleProxy;
        private ParticleSystem particle => particleProxy.Read();
        
        public override IEnumerator GetRoutine()
        {
            particle.Play();
            yield break;
        }
    }
}