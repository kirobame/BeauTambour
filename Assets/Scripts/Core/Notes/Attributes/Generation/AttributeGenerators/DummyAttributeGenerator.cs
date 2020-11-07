using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Percentages/Dummy")]
    [ItemName("Dummy by %")]
    public class DummyAttributeGenerator : WeightedNoteAttributeGenerator
    {
        public override NoteAttribute Generate() => new DummyAttribute();
    }
}