using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewAudioCharMapPackage", menuName = "Beau Tambour/General/Packages/Audio CharMap")]
    public class AudioCharMapPackage : AudioMapPackage<char> 
    {
        #region Encapsulated Types

        [Serializable]
        public class LetterAudioPair : KeyValuePair<char, AudioPackage> { }

        #endregion
        
        protected override KeyValuePair<char, AudioPackage>[] keyValuePairs => letterAudioPairs;
        [SerializeField] private LetterAudioPair[] letterAudioPairs;
    }
}