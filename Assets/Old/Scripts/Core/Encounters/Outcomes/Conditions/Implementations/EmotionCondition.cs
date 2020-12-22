using Flux;
using UnityEngine;

namespace Deprecated
{
    //[ItemPath("Is Emotion Met")]
    //[ItemName("Is Emotion Met")]
    public class EmotionCondition : Condition
    {
        [SerializeField] private CompoundEmotion target;

        public override bool IsMet(Encounter encounter, Note[] notes) => target.Matches(notes);
    }
}