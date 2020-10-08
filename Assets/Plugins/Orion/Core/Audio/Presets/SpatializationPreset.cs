using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class SpatializationPreset : AudioPreset
    {
        [SerializeField, Range(0, 5)] private float dopplerLevel = 1f;
        [SerializeField, MinMaxSlider(0, 1000)] private Vector2 range;
        
        public override void Apply(AudioSource source)
        {
            source.dopplerLevel =dopplerLevel;
            source.minDistance = range.x;
            source.maxDistance = range.y;
        }
    }
}