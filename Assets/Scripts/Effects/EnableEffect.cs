using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("General/Enable")]
    [ItemName("Enable")]
    public class EnableEffect : Effect
    {
        [SerializeField] private GameObject target;
        [SerializeField] private bool state;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            target.SetActive(state);
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}