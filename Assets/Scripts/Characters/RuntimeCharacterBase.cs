using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public abstract class RuntimeCharacterBase : MonoBehaviour
    {
        public abstract Animator Animator { get; }
        
        public abstract Character Asset { get; }
        
        public abstract Vector2 DialogueAnchor { get; }
        public abstract Vector2 SelectionAnchor { get; }
        public abstract Attach HeadSocket { get; }

        public abstract void Reboot();

        public abstract bool ActOut(Emotion emotion);
        public abstract bool PlayMelodyFor(Emotion emotion);

        public abstract bool BeginTalking();
        public abstract void StopTalking();
    }
}