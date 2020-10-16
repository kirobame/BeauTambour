using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Scaler : SerializedMonoBehaviour, ITweenable<Vector3>
    {
        public Vector3 Onset => Vector3.one * range.x;
        public Vector3 Outset => Vector3.one * range.y;

        [SerializeField] private Vector2 range;
        [SerializeField] private Transform target;

        [SerializeField] private IProxy<AnimationCurve> curveProxy;
        private AnimationCurve curve
        {
            get => curveProxy.Read();
            set => curveProxy.Write(value);
        }
        
        public void Tween(ITweenable<Vector3> tweenable, double ratio)
        {
            tweenable.Apply(Vector3.Lerp(tweenable.Onset, tweenable.Outset, curve.Evaluate((float)ratio)));
        }

        void ITweenable<Vector3>.Apply(Vector3 scale) => target.localScale = scale;
    }
}