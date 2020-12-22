using Flux;
using UnityEngine;

namespace Deprecated
{
    //[ItemPath("Percentages/Dummy")]
    //[ItemName("Dummy by %")]
    public class DummyAttributeGenerator : WeightedNoteAttributeGenerator
    {
        public override NoteAttribute Generate() => new DummyAttribute();
    }
}