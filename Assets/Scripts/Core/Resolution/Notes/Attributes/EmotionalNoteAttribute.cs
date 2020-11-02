namespace BeauTambour
{
    public class EmotionalNoteAttribute : NoteAttribute
    {
        public EmotionalNoteAttribute(NoteAttributeType emotion) => this.emotion = emotion;
        
        public override NoteAttributeType[] Keys => new NoteAttributeType[] {emotion};
        private NoteAttributeType emotion;

        public override string ToString() => $"(NoteAttribute) Emotion-{emotion}";
    }
}