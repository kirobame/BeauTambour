using UnityEngine;

namespace Flux
{
    [ItemPath("Utility/Marker")]
    public class Marker : Effect
    {
        public string Name => name;
        [SerializeField] private new string name;
    }
}