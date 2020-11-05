using UnityEngine;

namespace BeauTambour
{
    public class ParticleColorTarget : ColorTarget
    {
        [SerializeField] private ParticleSystem particle;
        
        public override void Set(Color color)
        {
            var main = particle.main;
            main.startColor = new ParticleSystem.MinMaxGradient(color);
        }
        public override void SetAlpha(float alpha)
        {
            var main = particle.main;
            
            var color = main.startColor.color;
            color.a = alpha;
            
            main.startColor = new ParticleSystem.MinMaxGradient(color);
        }
    }
}