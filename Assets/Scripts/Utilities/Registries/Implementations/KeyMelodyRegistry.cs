using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewKeyMelodyRegistry", menuName = "Beau Tambour/General/Registries/Key Melody")]
    public class KeyMelodyRegistry : Registry<string, AudioClip>
    {
        #region Encapsulated Types

        [Serializable]
        public class KeyMelodyPair : KeyValuePair<string, AudioClip> { }

        #endregion

        protected override KeyValuePair<string, AudioClip>[] keyValuePairs => pairs;
        [SerializeField] private KeyMelodyPair[] pairs;
    }
}