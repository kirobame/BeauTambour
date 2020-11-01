using System.Collections.Generic;

namespace BeauTambour
{
    public class Note
    {
        public IReadOnlyList<NoteAttribute> Attributes => attributes;
        private NoteAttribute[] attributes;
    }
}