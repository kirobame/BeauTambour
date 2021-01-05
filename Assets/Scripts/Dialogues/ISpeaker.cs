namespace BeauTambour
{
    public interface ISpeaker
    {
        Actor Actor { get; }
        RuntimeCharacter RuntimeLink { get; }
        
        AudioCharMapPackage AudioCharMap { get; }
        
        Dialogue[] GetDialogues(Emotion emotion);
        bool IsArcEnded { get; }

        void BeginTalking();
        void StopTalking();

        void PlayMelodyFor(Emotion emotion);
    }
}