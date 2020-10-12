using System.Collections;
using System.Collections.Generic;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class MoveTween : SerializedMonoBehaviour
    {
        [SerializeField] private IProxy<AnimationCurve> curveProxy;
        private AnimationCurve curve
        {
            get => curveProxy.Read();
            set => curveProxy.Write(value);
        }

        public void Tween(ITweenable<Vector2> tweenable, double ratio)
        {
            tweenable.Apply(Vector2.Lerp(tweenable.Start, tweenable.End, curve.Evaluate((float)ratio)));
        }
    }
}