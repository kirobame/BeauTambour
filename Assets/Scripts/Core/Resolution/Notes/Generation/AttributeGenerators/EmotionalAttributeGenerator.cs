using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Percentages/Emotion")]
    public class EmotionalAttributeGenerator : WeightedNoteAttributeGenerator
    {
        [SerializeField] private NoteAttributeType emotion;
        
        public override NoteAttribute Generate() => new EmotionalNoteAttribute(emotion);
    }
}