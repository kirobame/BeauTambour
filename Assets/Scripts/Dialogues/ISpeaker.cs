using UnityEngine;

namespace BeauTambour
{
    public interface ISpeaker
    {
        Actor Actor { get; }
        bool HasArcEnded { get; }
        int Branches { get; }
        
        RuntimeCharacter RuntimeLink { get; }
        Animator Animator { get; }
        
        AudioCharMapPackage AudioCharMap { get; }
        
        Dialogue[] GetDialogues(Emotion emotion);
        
        void BeginTalking();
        void StopTalking();

        void PlayMelodyFor(Emotion emotion);
        void ActOut(Emotion emotion);
    }
}