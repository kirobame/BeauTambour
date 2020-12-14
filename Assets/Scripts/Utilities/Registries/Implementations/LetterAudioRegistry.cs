using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewLetterAudioRegistry", menuName = "Beau Tambour/General/Registries/Letter Audio")]
    public class LetterAudioRegistry : Registry<char, AudioClip>
    {
        #region Encapsulated Types

        [Serializable]
        public class LetterAudioPair : KeyValuePair<char, AudioClip> { }

        #endregion

        protected override KeyValuePair<char, AudioClip>[] keyValuePairs => pairs;
        [SerializeField] private LetterAudioPair[] pairs;
    }
}