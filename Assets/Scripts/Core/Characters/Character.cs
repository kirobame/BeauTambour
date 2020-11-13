using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public abstract class Character : ScriptableObject
    {
        public Actor Name => actor;
        [SerializeField] private Actor actor;
    }
}