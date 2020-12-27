namespace BeauTambour
{
    public interface ISpeaker
    {
        Actor Actor { get; }
        RuntimeCharacter RuntimeLink { get; }
        
        AudioCharMapPackage AudioCharMap { get; }
        
        Dialogue GetDialogue(Emotion emotion);

        void BeginTalking();
        void StopTalking();
    }
}