using UnityEngine;

namespace BeauTambour
{
    public class EmotionEffect : PoolableAnimation
    {
        [Space, SerializeField] private Emotion emotion;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private EmotionColorRegistry emotionColorRegistry;

        //void Awake() => spriteRenderer.color = emotionColorRegistry[emotion];
    }
}