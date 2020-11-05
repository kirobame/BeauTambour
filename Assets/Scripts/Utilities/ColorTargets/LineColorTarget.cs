using UnityEngine;

namespace BeauTambour
{
    public class LineColorTarget : ColorTarget
    {
        [SerializeField] private LineRenderer line;
        
        public override void Set(Color color)
        {
            var gradient = line.colorGradient;
            var colorKeys = new GradientColorKey[gradient.colorKeys.Length];
            
            for (var i = 0; i < colorKeys.Length; i++)
            {
                var source = gradient.colorKeys[i];
                colorKeys[i] = new GradientColorKey(color, source.time);
            }
            
            gradient.SetKeys(colorKeys, gradient.alphaKeys);
            line.colorGradient = gradient;
        }
        public override void SetAlpha(float alpha)
        {
            var gradient = line.colorGradient;
            var alphaKeys = new GradientAlphaKey[gradient.alphaKeys.Length];
            
            for (var i = 0; i < alphaKeys.Length; i++)
            {
                var source = gradient.alphaKeys[i];
                alphaKeys[i] = new GradientAlphaKey(alpha, source.time);
            }
            
            gradient.SetKeys(gradient.colorKeys, alphaKeys);
            line.colorGradient = gradient;
        }
    }
}