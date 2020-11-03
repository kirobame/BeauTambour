using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class PoolableLineRenderer : Poolable<LineRenderer>
    {
        #region MyRegion

        [EnumAddress]
        public enum EventType
        {
            OnFillStart,
            OnFillUpdate,
            OnFillEnd,
        }

        #endregion
        
        public bool IsDone => Mathf.Abs(goal - Current) <= 0.0125f;
        public float Current => Value.colorGradient.alphaKeys[0].time;
        
        private float goal;
        private float velocity;

        void Awake()
        {
            Event.Open<Vector3>(EventType.OnFillStart, gameObject);
            Event.Open<Vector3>(EventType.OnFillUpdate, gameObject);
            Event.Open<Vector3>(EventType.OnFillEnd, gameObject);
        }
        
        public void SetPoints(Vector3[] points)
        {
            Value.positionCount = points.Length;
            Value.SetPositions(points);
            
            Event.CallLocal<Vector3>(EventType.OnFillUpdate, gameObject, Value.GetPosition(0));
        }

        public void StartFill()
        {
            var gradient = Value.colorGradient;
            gradient.SetKeys(gradient.colorKeys, new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(0f, 1f), 
            });
            Value.colorGradient = gradient;
            
            Event.CallLocal<Vector3>(EventType.OnFillStart, gameObject, Value.GetPosition(0));
        }

        public void TryUpdateFill(float smoothing) => TryUpdateFill(smoothing, goal);
        public void TryUpdateFill(float smoothing, float newTime)
        {
            if (Current >= newTime) return;
            goal = newTime;
                
            var gradient = Value.colorGradient;
            gradient.SetKeys(gradient.colorKeys, new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, Mathf.SmoothDamp(Current, goal, ref velocity, smoothing)), 
                new GradientAlphaKey(0f, 1f), 
            });
            Value.colorGradient = gradient;

            var index = Mathf.Clamp(Mathf.RoundToInt(Current * Value.positionCount), 0, Value.positionCount - 1);
            Event.CallLocal<Vector3>(EventType.OnFillUpdate, gameObject, Value.GetPosition(index));
        }

        public void EndFill()
        {
            var index = Mathf.Clamp(Mathf.RoundToInt(Current * Value.positionCount), 0, Value.positionCount - 1);
            Event.CallLocal<Vector3>(EventType.OnFillEnd, gameObject, Value.GetPosition(index));
        }
    }
}