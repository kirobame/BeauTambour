using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "IconByEmotion", menuName = "Beau Tambour/General/Registries/Emotion Icon")]
    public class EmotionIconRegistry : Registry<Emotion, Sprite>
    {
        #region Encapsulated Types

        [Serializable]
        public class EmotionIconPair : KeyValuePair<Emotion, Sprite> { }

        #endregion

        protected override KeyValuePair<Emotion, Sprite>[] keyValuePairs => pairs;
        [SerializeField] private EmotionIconPair[] pairs;
    }
}