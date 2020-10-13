using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Twirler : TimedEffect
    {
        [SerializeField, PropertyOrder(-1), MinMaxSlider(-90f,90f)] private Vector2 range;
        
        private Quaternion start;
        private Quaternion end;
        
        protected override void Start()
        {
            base.Start();

            start = Quaternion.AngleAxis(range.x, Vector3.forward);
            end = Quaternion.AngleAxis(range.y, Vector3.forward);
        }

        protected override void Execute(float ratio)
        {
            if (ratio < 0.5f) transform.localRotation = Quaternion.Lerp(start, end, ratio / 0.5f);
            else transform.localRotation = Quaternion.Lerp(end, start, (ratio - 0.5f) / 0.5f);
        }
    }
}