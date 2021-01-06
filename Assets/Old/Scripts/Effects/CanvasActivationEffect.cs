﻿using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class CanvasActivationEffect : Effect
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool state;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            canvasGroup.blocksRaycasts = state;
            canvasGroup.interactable = state;
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}