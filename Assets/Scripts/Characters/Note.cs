namespace BeauTambour
{
    public class Note
    {
        public Character speaker;
        public Emotion emotion;

        public void Clear()
        {
            speaker = null;
            emotion = Emotion.None;
        }
    }
}