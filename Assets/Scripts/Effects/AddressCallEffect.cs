using System.Collections.Generic;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [ItemPath("General/Call Address")]
    [ItemName("Call Address")]
    public class AddressCallEffect : Effect
    {
        [SerializeField] private EnumAddress address;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            Event.Call(address.Get());
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}