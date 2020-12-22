using UnityEngine;

namespace Deprecated
{
    public abstract class NoteAttributeGenerator : ScriptableObject
    {
        public abstract bool TryGenerate(out NoteAttribute attribute);
    }
}