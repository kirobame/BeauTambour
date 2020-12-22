using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEditor;
using UnityEngine;

namespace Deprecated
{
    public abstract class AnimationEffect : Effect
    {
        [SerializeField] private Animator target;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            Handle(target);
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }

        protected abstract void Handle(Animator animator);
    }
}