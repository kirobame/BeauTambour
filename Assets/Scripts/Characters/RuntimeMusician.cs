using System;
using UnityEngine;

namespace BeauTambour
{
    public class RuntimeMusician : RuntimeCharacter
    {
        public override Animator Animator => Intermediary.Animator;

        public SpeakerIntermediary Intermediary => intermediary;
        [Space, SerializeField] private SpeakerIntermediary intermediary;
        
        void Awake() => intermediary.SetSource(this);
        protected override void Start() => asset.Bootup(this, true);
        
        public override void ActOut(Emotion emotion) => intermediary.ActOut(emotion);

        public override void BeginTalking() => intermediary.BeginTalking();
        public override void StopTalking() => intermediary.StopTalking();

        public override void PlayMelodyFor(Emotion emotion) => intermediary.PlayMelodyFor(emotion);
    }
}