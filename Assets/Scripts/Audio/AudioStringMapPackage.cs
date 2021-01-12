using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewAudioStringMapPackage", menuName = "Beau Tambour/General/Packages/Audio StringMap")]
    public class AudioStringMapPackage : AudioMapPackage<string>
    {
        #region Encapsulated Types

        [Serializable]
        public class LetterAudioPair : KeyValuePair<string, AudioPackage> { }

        #endregion

        protected override KeyValuePair<string, AudioPackage>[] keyValuePairs => letterAudioPairs;
        [SerializeField] private LetterAudioPair[] letterAudioPairs;
    }
}