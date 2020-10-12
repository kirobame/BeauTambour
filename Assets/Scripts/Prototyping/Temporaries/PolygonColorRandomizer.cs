using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class PolygonColorRandomizer : MonoBehaviour, ITweenable<float>
    {
        private static List<Color> availableColors = new List<Color>();

        float ITweenable<float>.Start => 1f;
        float ITweenable<float>.End => 0f;
        
        [SerializeField] private RegularPolygon polygon;
        [SerializeField] private Color[] colors;

        void Awake()
        {
            if (!availableColors.Any())
            {
                foreach (var color in colors)
                {
                    availableColors.Add(color);
                }
            }
        }
        
        void Start()
        {
            var index = Random.Range(0, availableColors.Count);
            polygon.Color = availableColors[index];
            
            availableColors.RemoveAt(index);
        }

        void ITweenable<float>.Apply(float value)
        {
            var color = polygon.Color;
            color.a = value;

            polygon.Color = color;
        }
    }
}