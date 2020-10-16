using Shapes;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class BlockInitializer : MonoBehaviour, ITweenable<float>
    {
        public float Onset => 0f;
        public float Outset => 1f;

        [SerializeField] private Rectangle holder;
        [SerializeField] private SpriteRenderer icon;

        [Space, SerializeField] private ColorRegistry colorRegistry;
        [SerializeField] private ShapeRegistry shapeRegistry;

        private float iconMaxAlpha;

        void Awake() => iconMaxAlpha = icon.color.a;
        
        public void Execute(Color color, Shape shape)
        {
            holder.Color = colorRegistry[color];
            icon.sprite = shapeRegistry[shape];
        }

        void ITweenable<float>.Apply(float value)
        {
            var color = holder.Color;
            color.a = value;
            holder.Color = color;

            color = icon.color;
            color.a = value * iconMaxAlpha;
            icon.color = color;
        }
    }
}