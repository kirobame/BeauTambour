using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "IconByMusician", menuName = "Beau Tambour/General/Registries/Musician Icon")]
    public class MusicianIconRegistry : Registry<Actor,Sprite>
    {
        #region Encapsulated Types

        [Serializable]
        public class EmotionColorPair : KeyValuePair<Actor, Sprite> { }

        #endregion

        protected override KeyValuePair<Actor, Sprite>[] keyValuePairs => pairs;
        [SerializeField] private EmotionColorPair[] pairs;
    }
}