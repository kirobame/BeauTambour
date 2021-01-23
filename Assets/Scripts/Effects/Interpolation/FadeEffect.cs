using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Interpolation/Fade")]
    [ItemName("Fade")]
    public class FadeEffect : InterpolationEffect
    {
        [SerializeField] private AlphaTarget target;
        [SerializeField] private float goal;
        
        protected override void Startup() => target.Prepare(goal);
        protected override void Execute(float ratio) => target.Set(ratio);

        protected override void End() => target.End();
    }
}