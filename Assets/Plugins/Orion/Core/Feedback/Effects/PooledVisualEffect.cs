using System.Collections;
using UnityEngine;

namespace Orion
{
    public class PooledVisualEffect : Feedback
    {
        [SerializeField] private IReadable<IProvider<ParticleSystem>> particleProvider;

        public override IEnumerator GetRoutine()
        {
            var particle = particleProvider.Read().GetInstance();
            particle.Play();
            
            yield break;
        }
    }
}