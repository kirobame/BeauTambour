﻿using UnityEngine;

namespace BeauTambour
{
    public class RuntimeInterlocutor : RuntimeCharacter
    {
        public SpeakerIntermediary Intermediary => intermediary;
        [Space, SerializeField] private SpeakerIntermediary intermediary;

        public override void Reinitialize() => dialogueAnchor.localPosition = Vector3.zero;
    }
}