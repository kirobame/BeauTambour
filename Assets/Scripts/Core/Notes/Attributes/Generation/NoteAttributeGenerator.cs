using UnityEngine;

namespace BeauTambour
{
    public abstract class NoteAttributeGenerator : ScriptableObject
    {
        public abstract bool TryGenerate(out NoteAttribute attribute);
    }
}