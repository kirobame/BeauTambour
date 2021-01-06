using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "EffectByEmotion", menuName = "Beau Tambour/General/Registries/Emotion Effect")]
    public class EmotionEffectRegistry : Registry<Emotion, PoolableAnimation>
    {
        #region Encapsulated Types

        [Serializable]
        public class EmotionEffectPair : KeyValuePair<Emotion, PoolableAnimation> { }

        #endregion

        protected override KeyValuePair<Emotion, PoolableAnimation>[] keyValuePairs => pairs;
        [SerializeField] private EmotionEffectPair[] pairs;
    }
}