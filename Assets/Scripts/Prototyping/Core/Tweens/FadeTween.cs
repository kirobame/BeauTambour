using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class FadeTween : SerializedMonoBehaviour
    {
        [SerializeField] private IProxy<AnimationCurve> curveProxy;
        private AnimationCurve curve
        {
            get => curveProxy.Read();
            set => curveProxy.Write(value);
        }

        private float midPoint;

        void Start() => midPoint = curve.Evaluate(0.5f);
        
        public void Tween(ITweenable<float> tweenable, double ratio)
        {
            if (ratio < 0.5f)
            {
                var time = curve.Evaluate((float)ratio) / midPoint;
                tweenable.Apply(Mathf.Lerp(tweenable.Start, tweenable.End, time));
            }
            else
            {
                var time = (curve.Evaluate((float)ratio) - midPoint) / (1f - midPoint);
                tweenable.Apply(Mathf.Lerp(tweenable.End, tweenable.Start, time));
            }
        }
    }
}