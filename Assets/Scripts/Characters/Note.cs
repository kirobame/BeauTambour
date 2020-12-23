namespace BeauTambour
{
    public class Note
    {
        public ISpeaker speaker;
        public Emotion emotion;

        public void Clear()
        {
            speaker = null;
            emotion = Emotion.None;
        }
    }
}