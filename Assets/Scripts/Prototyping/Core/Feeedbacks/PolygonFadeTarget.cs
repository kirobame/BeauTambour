using Shapes;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class PolygonFadeTarget : MonoBehaviour, ITweenable<float>
    {
        public float Onset => 0;
        public float Outset => 1;

        [SerializeField] private RegularPolygon polygon;

        public void Apply(float value)
        {
            var color = polygon.Color;
            color.a = value;

            polygon.Color = color;
        }
    }
}