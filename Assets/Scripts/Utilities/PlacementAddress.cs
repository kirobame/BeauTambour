using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class PlacementAddress : MonoBehaviour
    {
        [SerializeField] private Transform value;

        [Space, SerializeField] private int group;
        [SerializeField] private string prefix;
        [SerializeField] private int index;

        void Awake() => Repository.Reference(value, $"{group}.{prefix}.{index}");
    }
}