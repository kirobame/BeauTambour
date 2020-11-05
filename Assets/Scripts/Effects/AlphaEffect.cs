using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Beau Tambour/Alpha")]
    public class AlphaEffect : InterpolationEffect
    {
        [SerializeField] private ColorTarget target;

        protected override void Execute(float ratio) => target.SetAlpha(ratio);
    }
}