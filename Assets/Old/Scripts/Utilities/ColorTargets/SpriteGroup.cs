using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deprecated
{
    public class SpriteGroup : ColorTarget
    {
        public override Color StartingColor => sprites.First().color;
        public override float StartingAlpha => sprites.First().color.a;

        public SpriteRenderer this[int index] => sprites[index];

        public IReadOnlyList<SpriteRenderer> Sprites => sprites;
        [SerializeField] private SpriteRenderer[] sprites;

        public override void Set(Color color)
        {
            foreach (var sprite in sprites) sprite.color = color;
        }

        public override void SetAlpha(float alpha)
        {
            foreach (var sprite in sprites)
            {
                var color = sprite.color;
                color.a = alpha;

                sprite.color = color;
            }
        }
    }
}