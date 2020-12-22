namespace Deprecated
{
    public class DummyAttribute : NoteAttribute
    {
        public override bool Equals(NoteAttribute other) => other is DummyAttribute;
    }
}