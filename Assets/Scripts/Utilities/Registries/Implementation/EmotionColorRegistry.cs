﻿using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "ColorByEmotion", menuName = "Beau Tambour/General/Registries/Emotion Color")]
    public class EmotionColorRegistry : Registry<Emotion, Color>
    {
        #region Encapsulated Types

        [Serializable]
        public class EmotionColorPair : KeyValuePair<Emotion, Color> { }

        #endregion

        protected override KeyValuePair<Emotion, Color>[] keyValuePairs => pairs;
        [SerializeField] private EmotionColorPair[] pairs;
    }
}