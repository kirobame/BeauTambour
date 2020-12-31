using UnityEngine;

namespace BeauTambour
{
    public class RuntimeInterlocutor : RuntimeCharacter
    {
        public SpeakerIntermediary Intermediary => intermediary;
        [Space, SerializeField] private SpeakerIntermediary intermediary;
    }
}