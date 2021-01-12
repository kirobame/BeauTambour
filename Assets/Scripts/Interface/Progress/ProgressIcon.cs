using UnityEngine;
using UnityEngine.UI;

namespace BeauTambour
{
    public class ProgressIcon : MonoBehaviour
    {
        public RectTransform RectTransform => transform as RectTransform;
        
        [SerializeField] private Image image;

        [Space, SerializeField] private EmotionColorRegistry emotionColorRegistry;
        [SerializeField] private EmotionIconRegistry emotionIconRegistry;

        public void Set(Emotion emotion)
        {
            RectTransform.localScale = Vector3.one;
            
            image.color = emotionColorRegistry[emotion];
            image.sprite = emotionIconRegistry[emotion];
        }
    }
}