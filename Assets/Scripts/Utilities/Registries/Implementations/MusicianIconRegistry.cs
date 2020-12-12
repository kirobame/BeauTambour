using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewMusicianIconRegistry", menuName = "Beau Tambour/General/Registries/Musician Icon")]
    public class MusicianIconRegistry : Registry<Musician, Sprite>
    {
        #region Encapsulated Types

        [Serializable]
        public class MusicianIconPair : KeyValuePair<Musician, Sprite> { }

        #endregion

        protected override KeyValuePair<Musician, Sprite>[] keyValuePairs => pairs;
        [SerializeField] private MusicianIconPair[] pairs;
    }
}