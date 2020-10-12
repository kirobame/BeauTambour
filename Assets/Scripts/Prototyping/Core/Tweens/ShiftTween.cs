using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ShiftTween : SerializedMonoBehaviour
    {
        [SerializeField] private IProxy<AnimationCurve> curveProxy;
        private AnimationCurve curve
        {
            get => curveProxy.Read();
            set => curveProxy.Write(value);
        }

        private float midPoint;

        void Start() => midPoint = curve.Evaluate(0.5f);
        
        public void Tween(ITweenable<Vector2> tweenable, double ratio)
        {
            var direction = (tweenable.Start - tweenable.End).normalized;
            direction *= Repository.Get<PlayArea>().TileSize;
            
            if (ratio < 0.5f)
            {
                var time = curve.Evaluate((float)ratio) / midPoint;
                tweenable.Apply(Vector2.Lerp(tweenable.Start, tweenable.Start + direction, time));
            }
            else
            {
                var time = (curve.Evaluate((float)ratio) - midPoint) / (1f - midPoint);
                tweenable.Apply(Vector2.Lerp(tweenable.End - direction, tweenable.End, time));
            }
        }
    }
}