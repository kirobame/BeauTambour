using Flux;
using UnityEngine;

namespace Deprecated
{
    public class AlphaEffect : InterpolationEffect
    {
        [SerializeField] private ColorTarget target;
        [SerializeField] private float goal;

        public override void Initialize()
        {
            base.Initialize();
            target.Initialize();
        }

        protected override void Execute(float ratio) => target.SetAlpha(Mathf.Lerp(target.StartingAlpha, goal, ratio));
    }
}