﻿using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    [ItemPath("Utility/Wait")]
    public class Wait : Effect
    {
        [SerializeField] private float time;

        private float runtime;

        public override void Initialize()
        {
            time = Mathf.Abs(time);
            runtime = 0f;
        }

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
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
}