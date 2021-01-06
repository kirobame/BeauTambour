using System;
using UnityEngine;

namespace BeauTambour
{
    public class RuntimeMusician : RuntimeCharacter
    {
        public SpeakerIntermediary Intermediary => intermediary;
        [Space, SerializeField] private SpeakerIntermediary intermediary;

        void Awake() => intermediary.SetSource(this);
    }
}