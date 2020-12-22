using Flux;
using UnityEngine;

namespace Deprecated
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool IsMet(Encounter encounter, Note[] notes);
    }
}