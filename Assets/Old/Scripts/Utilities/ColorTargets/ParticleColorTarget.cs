using UnityEngine;

namespace Deprecated
{
    public class ParticleColorTarget : ColorTarget
    {
        [SerializeField] private ParticleSystem particle;

        public override Color StartingColor => startingColor;
        public override float StartingAlpha => startingAlpha;

        private Color startingColor;
        private float startingAlpha;
        
        public override void Initialize()
        {
            var gradient = particle.main.startColor;
            startingColor = gradient.color;
            startingAlpha = gradient.color.a;
        }

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