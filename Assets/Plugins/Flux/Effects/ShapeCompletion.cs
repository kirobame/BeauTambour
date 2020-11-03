using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Beau Tambour/Shape Completion")]
    public class ShapeCompletion : Effect
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [SerializeField] private AnimationCurve alphaCurve;
        [SerializeField] private float time;
        
        private float runtime;
        
        private float[] starts;
        private float start;
        
        public override void Initialize()
        {
            time = Mathf.Abs(time);
            runtime = 0f;
            
            var gradient = lineRenderer.colorGradient;
            starts = new float[gradient.alphaKeys.Length];
            for (var i = 0; i < starts.Length; i++) starts[i] = gradient.alphaKeys[i].alpha;

            start = spriteRenderer.color.a;
        }
        
        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            var ratio = alphaCurve.Evaluate(Mathf.Clamp01(runtime / time));
            
            var gradient = lineRenderer.colorGradient;
            var alphaKeys = new GradientAlphaKey[gradient.alphaKeys.Length];
            
            for (var i = 0; i < alphaKeys.Length; i++)
            {
                var source = gradient.alphaKeys[i];
                var alpha = Mathf.Lerp(starts[i], 0f, ratio);
                
                alphaKeys[i] = new GradientAlphaKey(alpha, source.time);
            }
            
            gradient.SetKeys(gradient.colorKeys, alphaKeys);
            lineRenderer.colorGradient = gradient;

            var color = spriteRenderer.color;
            color.a = Mathf.Lerp(start, 0f, ratio);
            spriteRenderer.color = color;
            
            runtime += deltaTime;
            if (runtime < time)
            {
                prolong = false;
                return advancement;
            }
            else return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}