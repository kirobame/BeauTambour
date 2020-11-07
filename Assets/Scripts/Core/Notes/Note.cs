using System;
using System.Collections.Generic;
using System.Linq;
using Flux;

namespace BeauTambour
{
    public class Note
    {
        public Note(IEnumerable<NoteAttribute> attributes) => this.attributes = new HashSet<NoteAttribute>(attributes);

        public IEnumerable<NoteAttribute> Attributes => attributes;
        private HashSet<NoteAttribute> attributes;
        
        public void Add(IEnumerable<NoteAttribute> attributes) { foreach (var attribute in attributes) this.attributes.Add(attribute); }
    }
}