using UnityEngine;

namespace BeauTambour
{
    public class ParticleAlphaTarget : AlphaTarget
    {
        private float start;
        
        [SerializeField] private ParticleSystem particle;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            
            var gradient = particle.main.startColor;
            start = gradient.color.a;
        }
        public override void Set(float ratio)
        {
            var main = particle.main;
            
            var color = main.startColor.color;
            color.a = Mathf.Lerp(start, goal, ratio);
            
            main.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }
}