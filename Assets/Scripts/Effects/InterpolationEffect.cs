using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public abstract class InterpolationEffect : Effect
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float time;
        [SerializeField] private bool passThrough;
        
        private float runtime;
        
        public override void Initialize()
        {
            time = Mathf.Abs(time);
            runtime = 0f;
        }

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            var ratio = curve.Evaluate(Mathf.Clamp01(runtime / time));
            Execute(ratio);

            if (passThrough)
            {
                runtime += deltaTime;
                if (runtime >= time) runtime = 0f;
                
                return base.Evaluate(advancement, registry, deltaTime, out prolong);
            }
            else
            {
                runtime += deltaTime;
                if (runtime < time)
                {
                    prolong = false;
                    return advancement;
                }
                else return base.Evaluate(advancement, registry, deltaTime, out prolong);
            }
        }

        protected abstract void Execute(float ratio);
    }
}