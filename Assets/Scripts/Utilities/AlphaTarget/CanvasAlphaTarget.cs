using UnityEngine;

namespace BeauTambour
{
    public class CanvasAlphaTarget : AlphaTarget
    {
        [SerializeField] private CanvasGroup canvas;

        private float start;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            start = canvas.alpha;
        }
        public override void Set(float ratio) => canvas.alpha = Mathf.Lerp(start, goal, ratio);
    }
}