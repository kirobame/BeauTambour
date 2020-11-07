namespace BeauTambour
{
    public class MusicianAttribute : NoteAttribute
    {
        public MusicianAttribute(Musician musician) => this.musician = musician;

        public Musician Musician => musician;
        private Musician musician;

        public override bool Equals(NoteAttribute other) => other is MusicianAttribute emotionAttribute && emotionAttribute.Musician == musician;
        public override string ToString() => $"(MusicianAttribute) : {musician}";
    }
}