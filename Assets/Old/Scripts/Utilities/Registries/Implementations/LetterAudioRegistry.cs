using System;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewLetterAudioRegistry", menuName = "Beau Tambour/General/Registries/Letter Audio")]
    public class LetterAudioRegistry : Registry<char, AudioClip>
    {
        #region Encapsulated Types

        [Serializable]
        public class LetterAudioPair : KeyValuePair<char, AudioClip> { }

        #endregion

        public float Volume => volume;
        [SerializeField, Range(0,1)] private float volume;

        protected override KeyValuePair<char, AudioClip>[] keyValuePairs => pairs;
        [SerializeField] private LetterAudioPair[] pairs;
    }
}