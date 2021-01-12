using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using System;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "VowelSoundRegistry", menuName = "Beau Tambour/General/Registries/Vowel Sound")]
    public class VowelSoundRegistry : Registry<string,string>
    {
        #region Encapsulated Types

        [Serializable]
        public class VowelSoundPair : KeyValuePair<string, string> { }

        #endregion

        protected override KeyValuePair<string, string>[] keyValuePairs => pairs;
        [SerializeField] private VowelSoundPair[] pairs;
    }
}