using System;

namespace BeauTambour
{
    public abstract class NoteAttribute : IEquatable<NoteAttribute>
    {
        public abstract bool Equals(NoteAttribute other);
    }
}