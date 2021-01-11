using UnityEngine;

namespace BeauTambour
{
    public interface ISpeaker
    {
        int Id { get; }
        
        Actor Actor { get; }
        bool HasArcEnded { get; }
        int Branches { get; }
        
        RuntimeCharacter RuntimeLink { get; }
        Animator Animator { get; }
        
        AudioCharMapPackage AudioCharMap { get; }

        bool IsValid(Emotion emotion, out int selection, out int followingBranches);
        Dialogue[] GetDialogues(Emotion emotion);
        
        void BeginTalking();
        void StopTalking();

        void PlayMelodyFor(Emotion emotion);
        void ActOut(Emotion emotion);
    }
}