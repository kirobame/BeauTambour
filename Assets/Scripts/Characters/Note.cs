namespace BeauTambour
{
    public class Note
    {
        public Musician musician;
        public Emotion emotion;

        public void Clear()
        {
            musician = null;
            emotion = Emotion.None;
        }
    }
}