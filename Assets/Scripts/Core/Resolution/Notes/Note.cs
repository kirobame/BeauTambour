using System;
using System.Collections.Generic;
using System.Linq;
using Flux;

namespace BeauTambour
{
    public class Note
    {
        public Note(NoteAttribute[] attributes) => this.attributes = attributes;
        
        public IReadOnlyList<NoteAttribute> Attributes => attributes;
        private NoteAttribute[] attributes;

        public void Evaluate(IReadOnlyDictionary<NoteAttributeType, List<OutcomePort>> registry)
        {
            foreach (var attribute in attributes)
            {
                foreach (var key in attribute.Keys)
                {
                    if (!registry.TryGetValue(key, out var list)) continue;
                    foreach (var port in list) port.TryAdvance(attribute, key);
                }
            }
        }
    }
}