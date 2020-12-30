using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class EmotionAnchor : MonoBehaviour
    {
        public Shape Shape => shape;

        [SerializeField] private Shape shape;
        [SerializeField] private SpriteRenderer emotionRenderer;

        [Space, SerializeField] private float maxScale;
        [SerializeField] private Vector2 opacityRange;
        [SerializeField] private AnimationCurve interpolation;

        private float minScale;
        
        void Start()
        {
            shape.GenerateRuntimePoints();
            minScale = transform.localScale.x;

            var colorByEmotion = Repository.GetSingle<EmotionColorRegistry>(References.ColorByEmotion);
            var wantedColor = colorByEmotion[shape.Emotion];
            
            var color = emotionRenderer.color;
            color.r = wantedColor.r;
            color.g = wantedColor.g;
            color.b = wantedColor.b;

            emotionRenderer.color = color;
        }

        public void Reboot()
        {
            transform.localScale = Vector3.one * minScale;
            
            var color = emotionRenderer.color;
            color.a = 0;
            emotionRenderer.color = color;
        }

        public bool IsMagnetizing(Vector2 center, Vector2 input, float angleRange, float catchDistance)
        {
            var direction = (Vector2)transform.position - center;
            
            angleRange /= 180.0f;
            catchDistance *= direction.magnitude;
            
            var angle = 1.0f - Mathf.Clamp(Vector2.Angle(direction, input), 0.0f, 90.0f) / 90.0f;
            
            var angleRatio = 1.0f - Mathf.Clamp01((1.0f - angle) / angleRange);
            var distanceRatio = Mathf.Clamp01(input.magnitude / catchDistance) * angleRatio;
            var ratio = interpolation.Evaluate(distanceRatio);

            transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, ratio);
            var color = emotionRenderer.color;
            color.a = Mathf.Lerp(opacityRange.x, opacityRange.y, ratio);
            emotionRenderer.color = color;

            return angle >= 1.0f - angleRange && input.magnitude >= catchDistance;
        }

        public bool HasLostMagnetization(Vector2 center, Vector2 input, float angleRange, float looseDistance)
        {
            var direction = (Vector2)transform.position - center;
            
            angleRange /= 180.0f;
            looseDistance *= direction.magnitude;
            
            var angle = 1.0f - Mathf.Clamp(Vector2.Angle(input, direction), 0.0f, 90.0f) / 90.0f;
            return angle <= 1.0f - angleRange || input.magnitude <= looseDistance;
        }
    }
}