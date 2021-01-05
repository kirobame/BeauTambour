using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "MelodyByEmotion", menuName = "Beau Tambour/General/Registries/Emotion Melody")]
    public class EmotionMelodyRegistry : Registry<Emotion, AudioPackage>
    {
        #region Encapsulated Types

        [Serializable]
        public class EmotionMelodyPair : KeyValuePair<Emotion, AudioPackage> { }

        #endregion

        protected override KeyValuePair<Emotion, AudioPackage>[] keyValuePairs => pairs;
        [SerializeField] private EmotionMelodyPair[] pairs;
    }
}