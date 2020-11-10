using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Beau Tambour/Scale")]
    [ItemName("Scaler")]
    public class ScaleEffect : InterpolationEffect
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 end;
        
        protected override void Execute(float ratio)
        {
            target.localScale = Vector3.Lerp(start, end, ratio);
        }
    }
}