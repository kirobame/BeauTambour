using UnityEngine;

namespace BeauTambour
{
    public class SpriteAlphaTarget : AlphaTarget
    {
        private float start;
        private int isActive;

        private float velocity;
        
        [SerializeField] private SpriteRenderer spriteRenderer;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            start = spriteRenderer.color.a;

            if (start < goal) isActive = 1;
            else isActive = 2;
        }
        public override void Set(float ratio)
        {
            var target = Mathf.Lerp(start, goal, ratio);
            var color = spriteRenderer.color;

            if (isActive == 0 || isActive == 1 && color.a < target || isActive == 2 && color.a > target)
            {
                color.a = target;
                spriteRenderer.color = color;
            }
        }
        public override void End()
        {
            isActive = 0;
        }

        public void TrySet(float alpha)
        {
            if (isActive != 0) return;
            
            var color = spriteRenderer.color;
            color.a = Mathf.SmoothDamp(color.a, alpha, ref velocity, 0.125f);
            spriteRenderer.color = color;
        }
    }
}