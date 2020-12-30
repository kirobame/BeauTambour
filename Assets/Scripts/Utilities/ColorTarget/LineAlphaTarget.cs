using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    public class LineAlphaTarget : AlphaTarget
    {
        private float start;
        
        [SerializeField] private LineRenderer line;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            start = line.colorGradient.alphaKeys.First().alpha;
        }
        public override void Set(float ratio)
        {
            var value = Mathf.Lerp(start, goal, ratio);
            
            var gradient = line.colorGradient;
            var alphaKeys = new GradientAlphaKey[gradient.alphaKeys.Length];
            
            for (var i = 0; i < alphaKeys.Length; i++)
            {
                var source = gradient.alphaKeys[i];
                alphaKeys[i] = new GradientAlphaKey(value, source.time);
            }
            
            gradient.SetKeys(gradient.colorKeys, alphaKeys);
            line.colorGradient = gradient;
        }
    }
}