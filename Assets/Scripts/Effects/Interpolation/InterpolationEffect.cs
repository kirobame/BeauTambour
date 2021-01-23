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

        private bool hasBeenStartedUp;
        private float runtime;
        
        public override void Initialize()
        {
            base.Initialize();
            
            time = Mathf.Abs(time);
            if (time == 0f) time = Mathf.Epsilon;

            hasBeenStartedUp = false;
            runtime = 0f;
        }

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            if (!hasBeenStartedUp)
            {
                Startup();
                hasBeenStartedUp = true;
            }

            var ratio = curve.Evaluate(Mathf.Clamp01(runtime / time));
            Execute(ratio);
            
            runtime += deltaTime;
            if (runtime < time)
            {
                prolong = false;
                return advancement;
            }
            else
            {
                Execute(1.0f);
                End();
                
                return base.Evaluate(advancement, registry, deltaTime, out prolong);
            }
        }

        protected abstract void Startup();
        protected abstract void Execute(float ratio);
        protected abstract void End();
    }
}