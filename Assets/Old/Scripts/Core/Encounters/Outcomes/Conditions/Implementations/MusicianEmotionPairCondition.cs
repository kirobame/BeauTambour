using Flux;
using UnityEngine;

namespace Deprecated
{
    //[ItemPath("Has Musician played Emotion")]
    //[ItemName("Has Musician played Emotion")]
    public class MusicianEmotionPairCondition : Condition
    {
        [SerializeField] private Musician musician;
        [SerializeField] private Emotion emotion;
        
        public override bool IsMet(Encounter encounter, Note[] notes)
        {
            foreach (var note in notes)
            {
                var isValid = 0;
                foreach (var attribute in note.Attributes)
                {
                    if (attribute is MusicianAttribute musicianAttribute && musicianAttribute.Musician == musician) isValid++;
                    if (attribute is EmotionAttribute emotionAttribute && emotionAttribute.Emotion == emotion) isValid++;
                }

                if (isValid == 2) return true;
            }

            return false;
        }
    }
}