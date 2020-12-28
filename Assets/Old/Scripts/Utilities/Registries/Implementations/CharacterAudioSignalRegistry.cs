using System;
using BeauTambour;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewCharacterAudioSignal", menuName = "Beau Tambour/General/Registries/Character Audio Signal")]
    public class CharacterAudioSignalRegistry : Registry<Character, AudioCollection>
    {
        #region Encapsulated Types

        [Serializable]
        public class CharacterAudioSignalPair : KeyValuePair<Character, AudioCollection> { }

        #endregion

        protected override KeyValuePair<Character, AudioCollection>[] keyValuePairs => pairs;
        [SerializeField] private CharacterAudioSignalPair[] pairs;
    }
}