namespace BeauTambour
{
    public interface ISpeaker
    {
        Actor Actor { get; }
        RuntimeCharacter RuntimeLink { get; }
        
        AudioCharMapPackage AudioCharMap { get; }
        
        Dialogue[] GetDialogues(Emotion emotion);

        void BeginTalking();
        void StopTalking();

        void PlayMelodyFor(Emotion emotion);
    }
}