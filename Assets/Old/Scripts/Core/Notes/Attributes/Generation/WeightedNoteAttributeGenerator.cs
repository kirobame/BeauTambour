using UnityEngine;

namespace Deprecated
{
    public abstract class WeightedNoteAttributeGenerator : NoteAttributeGenerator
    {
        [SerializeField, Range(0, 100)] private float percentage = 100;
        
        public override bool TryGenerate(out NoteAttribute attribute)
        {
            if (Random.Range(0, 101) <= percentage)
            {
                attribute = Generate();
                return true;
            }

            attribute = null;
            return false;
        }
        public abstract NoteAttribute Generate();
    }
}