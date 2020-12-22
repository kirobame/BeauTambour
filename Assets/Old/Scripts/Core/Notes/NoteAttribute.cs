using System;

namespace Deprecated
{
    public abstract class NoteAttribute : IEquatable<NoteAttribute>
    {
        public abstract bool Equals(NoteAttribute other);
    }
}