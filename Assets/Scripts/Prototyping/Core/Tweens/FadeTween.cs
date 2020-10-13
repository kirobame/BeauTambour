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

        public void Tween(ITweenable<float> tweenable, double ratio)
        {
            tweenable.Apply(Mathf.Lerp(tweenable.Onset, tweenable.Outset, curve.Evaluate((float)ratio)));
        }
    }
}