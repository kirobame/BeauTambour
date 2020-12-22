using UnityEngine;

namespace Deprecated
{
    public abstract class CharacterInput : ScriptableObject
    {
        public abstract Character Character { get; }
        
        public abstract void Execute();
    }
    public abstract class CharacterInput<T> : CharacterInput where T : Character
    {
        public override Character Character => target;

        [SerializeField] protected T target;
    }
}