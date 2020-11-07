using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Is Emotion Met")]
    [ItemName("Is Emotion Met")]
    public class EmotionCondition : Condition
    {
        [SerializeField] private CompoundEmotion target;
        
        public override bool IsMet(Encounter encounter, Note[] notes)
        {
            var emotionAttributes = notes.Query<EmotionAttribute>();
            
            var copy = target.GetCopy();
            var countdown = copy.Count;

            foreach (var emotionAttribute in emotionAttributes)
            {
                if (!copy.Remove(emotionAttribute.Emotion)) continue;
                
                countdown--;
                if (countdown <= 0) return true;
            }

            return false;
        }
    }
}