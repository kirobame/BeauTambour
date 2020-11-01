using UnityEngine;

namespace BeauTambour
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool IsMet();
    }
}