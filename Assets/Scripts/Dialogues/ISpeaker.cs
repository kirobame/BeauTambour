using UnityEngine;

namespace BeauTambour
{
    public interface ISpeaker
    {
        Actor Actor { get; }
        
        RuntimeCharacter RuntimeLink { get; }
        Animator Animator { get; }
        
        AudioStringMapPackage AudioStringMap { get; }
        
        Dialogue[] GetDialogues(Emotion emotion);
        bool IsArcEnded { get; }

        void BeginTalking();
        void StopTalking();

        void PlayMelodyFor(Emotion emotion);
        void ActOut(Emotion emotion);
    }
}