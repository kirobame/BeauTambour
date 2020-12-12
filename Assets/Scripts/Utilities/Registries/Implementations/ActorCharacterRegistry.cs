using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewActorCharacterRegistry", menuName = "Beau Tambour/General/Registries/Actor Character")]
    public class ActorCharacterRegistry : Registry<Actor, Character>
    {
        #region Encapsulated Types

        [Serializable]
        public class ActorCharacterPair : KeyValuePair<Actor, Character> { }

        #endregion

        protected override KeyValuePair<Actor, Character>[] keyValuePairs => pairs;
        [SerializeField] private ActorCharacterPair[] pairs;
    }
}