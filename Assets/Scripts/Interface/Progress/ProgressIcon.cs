using UnityEngine;
using UnityEngine.UI;

namespace BeauTambour
{
    public class ProgressIcon : MonoBehaviour
    {
        public RectTransform RectTransform => transform as RectTransform;
        public float Size => emotion != Emotion.None ? size : 1.0f;
        public Vector2 Offset => emotion != Emotion.None ? Vector2.zero : new Vector2(0, 8.0f);
        
        [SerializeField] private Image image;
        [SerializeField] private float size = 0.75f;
        
        [Space, SerializeField] private EmotionIconRegistry emotionIconRegistry;

        private Emotion emotion;

        public void Set(Emotion emotion)
        {
            this.emotion = emotion;
            RectTransform.localScale = Vector3.one * Size;

            image.sprite = emotionIconRegistry[emotion];
            image.SetNativeSize();
        }
    }
}