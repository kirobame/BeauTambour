using UnityEngine;

namespace BeauTambour
{
    public class SimpleAttach : Attach
    {
        public override Transform Value => value;
        [SerializeField] private Transform value;
    }
}