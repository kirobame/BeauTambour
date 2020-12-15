using System.Collections;
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
            
            OnMatch,
            OnFail
        }
        #endregion

        [SerializeField] private LineRenderer path;
        [SerializeField] private EmotionColorRegistry emotionColorRegistry;
        
        public bool IsDone => Mathf.Abs(goal - Current) <= 0.0125f;
        public float Current => (float)Value.positionCount / points.Length;
        
        private float goal;
        private float velocity;

        private Shape shape;
        private Vector3[] points;

        private Coroutine revealPathRoutine;

        void Awake()
        {
            Event.Open<Vector3>(EventType.OnShapeRecognized, gameObject);
            Event.Open<Vector3>(EventType.OnDraw, gameObject);
            Event.Open<Vector3>(EventType.OnCompletion, gameObject);
            
            Event.Open<Color>(EventType.OnColorAssigned, gameObject);

            Event.Open(EventType.OnMatch, gameObject);
            Event.Open(EventType.OnFail, gameObject);
        }

        public void AssignShape(Shape shape, int subDivision)
        {
            this.shape = shape;
            
            points = shape.GenerateCopy(subDivision);
            for (var i = 0; i < points.Length; i++) points[i] = transform.TransformPoint(points[i]);
            
            Value.positionCount = 1;
            Value.SetPosition(0, points[0]);

            revealPathRoutine = StartCoroutine(RevealPathRoutine(0.3f));
            path.positionCount = points.Length;
            path.SetPositions(points);
            
            Event.CallLocal<Vector3>(EventType.OnDraw, gameObject, Value.GetPosition(0));

            var color = emotionColorRegistry[shape.Emotion];
            color.r -= 0.21f;
            color.g -= 0.21f;
            color.b -= 0.21f;

            var colors = new GradientColorKey[]
            {
                new GradientColorKey(color, 0f),
                new GradientColorKey(color, 1f), 
            };
            var gradient = Value.colorGradient;
            
            gradient.SetKeys(colors, new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(1f, 1f), 
            });
            Value.colorGradient = gradient;

            Event.CallLocal<Color>(EventType.OnColorAssigned, gameObject, emotionColorRegistry[shape.Emotion]);
            Event.CallLocal<Vector3>(EventType.OnShapeRecognized, gameObject, Value.GetPosition(0));
        }

        private IEnumerator RevealPathRoutine(float goal)
        {
            Gradient gradient;
            
            var time = 0f;
            while (time < goal)
            {
                var alpha = time / goal;
                
                gradient = path.colorGradient;
                gradient.SetKeys(gradient.colorKeys, new GradientAlphaKey[]
                {
                    new GradientAlphaKey(alpha, 0f), 
                    new GradientAlphaKey(alpha, 1f), 
                });
                path.colorGradient = gradient;
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            
            gradient = path.colorGradient;
            gradient.SetKeys(gradient.colorKeys, new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(1f, 1f), 
            });
            path.colorGradient = gradient;

            revealPathRoutine = null;
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

        public void Complete(bool success)
        {
            if (revealPathRoutine != null)
            {
                StopCoroutine(revealPathRoutine);
                revealPathRoutine = null;
            }
            
            Event.CallLocal<Vector3>(EventType.OnCompletion, gameObject, Value.GetPosition(Value.positionCount - 1));
            
            if (success) Event.CallLocal(EventType.OnMatch, gameObject);
            else Event.CallLocal(EventType.OnFail, gameObject);
        }
    }
}