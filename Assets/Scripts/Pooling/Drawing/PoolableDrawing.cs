using System.Collections;
using Deprecated;
using Flux;
using UnityEngine;
using UnityScript.Lang;
using Array = System.Array;
using Event = Flux.Event;

namespace BeauTambour
{
    public class PoolableDrawing : Poolable<LineRenderer>
    {
        [SerializeField] private LineRenderer path;
        
        public bool IsDone => Mathf.Abs(goal - Current) <= 0.0125f;
        public float Current => (float)Value.positionCount / points.Length;

        private float goal;
        private float velocity;

        private Shape shape;
        private Vector3[] points;
        
        private Coroutine revealPathRoutine;

        protected override void Bootup()
        {
            Event.Open<Vector3>(GameEvents.OnDrawingStart, gameObject);
            Event.Open<Vector3>(GameEvents.OnDraw, gameObject);
            Event.Open<Vector3>(GameEvents.OnDrawingEnd, gameObject);
            Event.Open<Vector3>(GameEvents.OnDrawingCancelled, gameObject);

            Event.Open<Color>(GameEvents.OnDrawingColorReception, gameObject);
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

            Event.CallLocal(GameEvents.OnDraw, gameObject, Value.GetPosition(0));

            var colorByEmotion = Repository.GetSingle<EmotionColorRegistry>(References.ColorByEmotion);
            var color = colorByEmotion[shape.Emotion];

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

            Event.CallLocal(GameEvents.OnDrawingColorReception, gameObject, colorByEmotion[shape.Emotion]);
            Event.CallLocal(GameEvents.OnDrawingStart, gameObject, Value.GetPosition(0));
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

            Event.CallLocal(GameEvents.OnDraw, gameObject, Value.GetPosition(Value.positionCount - 1));
        }

        public void Stop()
        {
            if (revealPathRoutine != null)
            {
                StopCoroutine(revealPathRoutine);
                revealPathRoutine = null;
            }
            
            var index = Mathf.Clamp(Mathf.RoundToInt(goal * points.Length), 0, points.Length - 1);
            Event.CallLocal(GameEvents.OnDrawingCancelled, gameObject, Value.GetPosition(index));
        }
        public void Complete()
        {
            if (revealPathRoutine != null)
            {
                StopCoroutine(revealPathRoutine);
                revealPathRoutine = null;
            }
            
            Value.positionCount = points.Length;
            Value.SetPositions(points);
            
            Event.CallLocal(GameEvents.OnDrawingEnd, gameObject, Value.GetPosition(Value.positionCount - 1));
        }
    }
}