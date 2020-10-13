using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Bopper : TimedEffect
    {
        [SerializeField, PropertyOrder(-1)] private float height;
        
        protected override void Execute(float ratio)
        {
            var position = transform.localPosition;
            position.y = curve.Evaluate(ratio) * height;
            transform.localPosition = position;
        }
    }
}