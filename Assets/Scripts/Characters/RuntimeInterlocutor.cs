using UnityEngine;

namespace BeauTambour
{
    public class RuntimeInterlocutor : RuntimeCharacter
    {
        public override Animator Animator => Intermediary.Animator;
        
        public SpeakerIntermediary Intermediary => intermediary;
        [Space, SerializeField] private SpeakerIntermediary intermediary;

        void Awake() => intermediary.SetSource(this);
        
        public override void ActOut(Emotion emotion) => intermediary.ActOut(emotion);

        public override void BeginTalking() => intermediary.BeginTalking();
        public override void StopTalking() => intermediary.StopTalking();

        public override void PlayMelodyFor(Emotion emotion) => intermediary.PlayMelodyFor(emotion);
        
        public override void Reinitialize() => dialogueAnchor.localPosition = Vector3.zero;
    }
}