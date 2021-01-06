using UnityEngine;

namespace Utilities.Spine
{
    public class OutlineLookup : MonoBehaviour
    {
        public MeshFilter Filter => filter;
        [SerializeField] private MeshFilter filter;
        
        public Material Small => small;
        [SerializeField] private Material small;
        
        public Material Large => large;
        [SerializeField] private Material large;
    }
}