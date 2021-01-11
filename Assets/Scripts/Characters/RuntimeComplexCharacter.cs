using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class RuntimeComplexCharacter : RuntimeCharacter
    {
        public override Animator Animator => intermediary.Animator;
        
        [Space, SerializeField] private Musician secondAsset;
        [SerializeField] private SpeakerIntermediary intermediary;

        void Awake() => intermediary.SetSource(this);
        
        protected override void Start()
        {
            base.Start();
            
            Repository.Reference(this, $"Complex.{asset.Actor}");
            secondAsset.Bootup(this, false);
        }

        public void Switch()
        {
            GameState.RegisterSpeakerForUse(secondAsset);
        }
        
        public override void ActOut(Emotion emotion) => intermediary.ActOut(emotion);

        public override void BeginTalking() => intermediary.BeginTalking();
        public override void StopTalking() => intermediary.StopTalking();

        public override void PlayMelodyFor(Emotion emotion) => intermediary.PlayMelodyFor(emotion);
    }
}