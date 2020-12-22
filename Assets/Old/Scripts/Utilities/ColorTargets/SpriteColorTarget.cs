using UnityEngine;

namespace Deprecated
{
    public class SpriteColorTarget : ColorTarget
    {
        [SerializeField] private SpriteRenderer sprite;
        
        public override Color StartingColor => startingColor;
        public override float StartingAlpha => startingAlpha;
        
        private Color startingColor;
        private float startingAlpha;

        public override void Initialize()
        {
            startingColor = sprite.color;
            startingAlpha = sprite.color.a;
        }

        public override void Set(Color color) => sprite.color = color;
        public override void SetAlpha(float alpha)
        {
            var color = sprite.color;
            color.a = alpha;

            sprite.color = color;
        }
    }
}