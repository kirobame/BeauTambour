namespace BeauTambour
{
    public interface ISpeaker
    {
        Actor Actor { get; }
        RuntimeCharacter RuntimeLink { get; }
        
        Dialogue GetDialogue(Emotion emotion);
    }
}