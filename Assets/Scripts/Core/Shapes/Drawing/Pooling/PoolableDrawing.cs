using Flux;
using UnityEngine;
using UnityScript.Lang;
using Array = System.Array;
using Event = Flux.Event;

namespace BeauTambour
{
    public class PoolableDrawing : Poolable<LineRenderer>
    {
        #region MyRegion

        [EnumAddress]
        public enum EventType
        {
            OnShapeRecognized,
            OnDraw,
            OnCompletion,
            
            OnColorAssigned,
        }
        #endregion
        
        public bool IsDone => Mathf.Abs(goal - Current) <= 0.0125f;
        public float Current => (float)Value.positionCount / points.Length;
        
        private float goal;
        private float velocity;

        private Shape shape;
        private Vector3[] points;

        void Awake()
        {
            Event.Open<Vector3>(EventType.OnShapeRecognized, gameObject);
            Event.Open<Vector3>(EventType.OnDraw, gameObject);
            Event.Open<Vector3>(EventType.OnCompletion, gameObject);
            
            Event.Open<Color>(EventType.OnColorAssigned, gameObject);
        }

        public void AssignShape(Shape shape, int subDivision)
        {
            this.shape = shape;
            points = shape.GenerateCopy(subDivision);
            
            Value.positionCount = 1;
            Value.SetPosition(0, points[0]);
            
            Event.CallLocal<Vector3>(EventType.OnDraw, gameObject, Value.GetPosition(0));
            
            var colors = new GradientColorKey[]
            {
                new GradientColorKey(shape.Color, 0f),
                new GradientColorKey(shape.Color, 1f), 
            };
            var gradient = Value.colorGradient;
            
            gradient.SetKeys(colors, new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(1f, 1f), 
            });
            Value.colorGradient = gradient;

            Event.CallLocal<Color>(EventType.OnColorAssigned, gameObject, shape.Color);
            Event.CallLocal<Vector3>(EventType.OnShapeRecognized, gameObject, Value.GetPosition(0));
        }

        public void Draw(float smoothing) => Draw(smoothing, goal);
        public void Draw(float smoothing, float newTime)
        {
            if (Current >= newTime) return;
            goal = newTime;
            
            var index = Mathf.Clamp(Mathf.RoundToInt(newTime * points.Length), 0, points.Length - 1);
            Value.positionCount = index + 1;
            Value.SetPositions(points);

            Event.CallLocal<Vector3>(EventType.OnDraw, gameObject, Value.GetPosition(Value.positionCount - 1));
        }

        public void Complete() => Event.CallLocal<Vector3>(EventType.OnCompletion, gameObject, Value.GetPosition(Value.positionCount - 1));
    }
}