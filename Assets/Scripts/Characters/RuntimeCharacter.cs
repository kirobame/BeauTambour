using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public class RuntimeCharacter : MonoBehaviour
    {
        [SerializeField] private Character asset;

        public Vector3 DialogueAnchor => dialogueAnchor.position;
        [Space, SerializeField] protected Transform dialogueAnchor;

        public Vector3 TopCenter => topCenter.position;
        [SerializeField] protected Transform topCenter;

        public Socket HeadSocket => headSocket;
        [SerializeField] protected Socket headSocket;
        
        void Start() => asset.Bootup(this);

        public virtual void Reinitialize() { }
        
        public virtual void ActOut(Emotion emotion) { }
    }
}