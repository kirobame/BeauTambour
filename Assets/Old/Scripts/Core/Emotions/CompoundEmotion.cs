using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

namespace Deprecated
{
    //[IconIndicator(-1684237209172799180), CreateAssetMenu(fileName = "NewCompoundEmotion", menuName = "Beau Tambour/General/Compound Emotion")]
    public class CompoundEmotion : ScriptableObject
    {
        [SerializeField] private Emotion[] emotions;

        public List<Emotion> GetCopy() => new List<Emotion>(emotions);

        public bool Matches(Note[] notes)
        {
            var emotions = notes.Query<EmotionAttribute>().Select(attribute => attribute.Emotion);
            if (!emotions.Any()) return false;

            var selfComplexity = CountComplexity(this.emotions);
            var complexity = CountComplexity(emotions);

            if (complexity != selfComplexity) return false;
            
            var copy = GetCopy();
            foreach (var emotion in emotions)
            {
                if (copy.Remove(emotion) && copy.Count == 0) return true;
            }

            return false;
        }

        private int CountComplexity(IEnumerable<Emotion> emotions)
        {
            var uniques = new List<Emotion>();
            foreach (var emotion in emotions)
            {
                if (!uniques.Contains(emotion)) uniques.Add(emotion);
            }

            return uniques.Count;
        }
    }
}