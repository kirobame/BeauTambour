using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public abstract class Character : ScriptableObject
    {
        public string Name => name;
        [SerializeField] private new string name;
    }
}