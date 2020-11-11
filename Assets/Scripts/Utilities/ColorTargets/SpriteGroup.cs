using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    public class SpriteGroup : MonoBehaviour
    {
        public Color Color => sprites.First().color;

        public SpriteRenderer this[int index] => sprites[index];

        [SerializeField] private SpriteRenderer[] sprites;

        public void SetColor(Color color)
        {
            foreach (var sprite in sprites) sprite.color = color;
        }
        public void SetAlpha(float alpha)
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