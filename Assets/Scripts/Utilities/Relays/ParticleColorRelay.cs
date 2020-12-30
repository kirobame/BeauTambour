using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public class ParticleColorRelay : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        public void Set(Color value)
        {
            var main = particle.main;
            main.startColor = new ParticleSystem.MinMaxGradient(value);
        }
    }
}