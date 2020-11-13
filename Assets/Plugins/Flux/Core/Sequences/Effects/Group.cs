using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    [ItemPath("Utility/Group")]
    public class Group : Effect
    {
        [SerializeField] private int range;
        [SerializeField] private Effect end;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            for (var i = 0; i < range; i++)
            {
                var index = advancement + 1 + i;
                registry[index].Evaluate(index, registry, deltaTime, out var subProlong);
            }

            if (end.IsDone)
            {
                prolong = true;
                return advancement + range + 1;
            }
            else
            {
                prolong = false;
                return advancement;
            }
        }
    }
}