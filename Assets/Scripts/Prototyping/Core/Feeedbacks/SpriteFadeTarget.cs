using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class SpriteFadeTarget : MonoBehaviour, ITweenable<float>
    {
        public float Onset => 0;
        public float Outset => 1;

        [SerializeField] private SpriteRenderer sprite;

        public void Apply(float value)
        {
            var color = sprite.color;
            color.a = value;

            sprite.color = color;
        }
    }
}