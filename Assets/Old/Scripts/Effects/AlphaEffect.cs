using BeauTambour;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class AlphaEffect : InterpolationEffect
    {
        [SerializeField] private ColorTarget target;
        [SerializeField] private float goal;

        protected override void Startup() => target.Initialize();
        protected override void Execute(float ratio) => target.SetAlpha(Mathf.Lerp(target.StartingAlpha, goal, ratio));
        protected override void End()
        {
            throw new System.NotImplementedException();
        }
    }
}