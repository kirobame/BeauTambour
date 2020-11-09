using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Is Musician Involved")]
    [ItemName("Is Musician Involved")]
    public class MusicianCondition : Condition
    {
        [SerializeField] private Musician musician;
        
        public override bool IsMet(Encounter encounter, Note[] notes)
        {
            var emotionAttributes = notes.Query<MusicianAttribute>();
            return emotionAttributes.Any(attribute => attribute.Musician == musician);
        }
    }
}