using System.Collections.Generic;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class NoteInitializer : SerializedMonoBehaviour, ITweenable<float>
    {
        public float Onset => 0;
        public float Outset => 1;
        
        [SerializeField] private Note source;

        [SerializeField] private ShapeRegistry shapeRegistry;
        [SerializeField] private List<SpriteRenderer[]> targets = new List<SpriteRenderer[]>();
        [SerializeField, Range(0f,1f)] private float shade;

        private int index;
        
        public void Execute(UnityEngine.Color color)
        {
            var shadeFactor = shade;

            var splitShape = source.Shape.Split();
            index = splitShape.Length - 1;
            
            for (var i = 0; i < targets[index].Length; i++)
            {
                targets[index][i].gameObject.SetActive(true);
                targets[index][i].sprite = shapeRegistry[splitShape[i]];
                targets[index][i].color = color;
                
                color *= 1 - shadeFactor;
                color.a = 1;
                
                shadeFactor += shade;
            }
        }

        void ITweenable<float>.Apply(float value)
        {
            for (var i = 0; i < targets[index].Length; i++)
            {
                var color = targets[index][i].color;
                color.a = value;

                targets[index][i].color = color;
            }
        }
    }
}