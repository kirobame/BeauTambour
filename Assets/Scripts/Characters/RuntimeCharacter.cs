using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public class RuntimeCharacter : MonoBehaviour
    {
        public virtual Animator Animator { get; }
        
        public Character Asset => asset;
        [SerializeField] protected Character asset;

        public Vector3 DialogueAnchor => dialogueAnchor.position;
        [Space, SerializeField] protected Transform dialogueAnchor;

        public Vector3 TopCenter => topCenter.position;
        [SerializeField] protected Transform topCenter;

        public Attach HeadSocket => headSocket;
        [SerializeField] protected Attach headSocket;
        
        protected virtual void Start() => asset.Bootup(this);

        public virtual void Reinitialize() { }
        
        public virtual void ActOut(Emotion emotion) { }
        
        public virtual void BeginTalking() { }
        public virtual void StopTalking() { }

        public virtual void PlayMelodyFor(Emotion emotion) { }
    }
}