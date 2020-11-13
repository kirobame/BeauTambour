namespace BeauTambour
{
    public class EmotionAttribute : NoteAttribute
    {
        public EmotionAttribute(Emotion emotion) => this.emotion = emotion;

        public Emotion Emotion => emotion;
        private Emotion emotion;

        public override bool Equals(NoteAttribute other) => other is EmotionAttribute emotionAttribute && emotionAttribute.Emotion == emotion;
        public override string ToString() => $"(EmotionAttribute) : {emotion}";
    }
}