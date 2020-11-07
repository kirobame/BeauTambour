using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewMusician", menuName = "Beau Tambour/Characters/Musician")]
    public class Musician : Character
    {
        private CompoundAttributeGenerator attributeGenerator;
        //
        public void SetGenerator(CompoundAttributeGenerator generator) => attributeGenerator = generator;
        public IEnumerable<NoteAttribute> Prompt()
        {
            var attributes = new List<NoteAttribute>() {new MusicianAttribute(this)};
            return attributes.Concat(attributeGenerator.Generate());
        }
    }
}