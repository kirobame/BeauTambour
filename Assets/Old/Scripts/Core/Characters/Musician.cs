using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewMusician", menuName = "Beau Tambour/Characters/Musician")]
    public class Musician : Character
    {
        private CompoundAttributeGenerator attributeGenerator;
        //
        public void SetGenerator(CompoundAttributeGenerator generator) => attributeGenerator = generator;
        public IEnumerable<NoteAttribute> Prompt()
        {
            var attributes = new List<NoteAttribute>() {new MusicianAttribute(this)};
            
            if (attributeGenerator != null) return attributes.Concat(attributeGenerator.Generate());
            else return attributes;
        }
    }
}