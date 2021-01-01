using System.Collections.Generic;
using Flux;
using UnityEngine;
using UnityEngine.Audio;

namespace BeauTambour
{
    [ItemPath("General/Activate Interface")]
    [ItemName("Activate Interface")]
    public class ActivateInterfaceEffect : Effect
    {
        [SerializeField] private CanvasGroup target;
        [SerializeField] private bool state;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            target.blocksRaycasts = state;
            target.interactable = state;

            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}