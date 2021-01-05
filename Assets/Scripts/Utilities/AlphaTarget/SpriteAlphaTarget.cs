using UnityEngine;

namespace BeauTambour
{
    public class SpriteAlphaTarget : AlphaTarget
    {
        private float start;
        
        [SerializeField] private SpriteRenderer spriteRenderer;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            start = spriteRenderer.color.a;
        }
        public override void Set(float ratio)
        {
            var color = spriteRenderer.color;
            color.a = Mathf.Lerp(start, goal, ratio);
            spriteRenderer.color = color;
        }
    }
}