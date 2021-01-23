using UnityEngine;

namespace BeauTambour
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private Behaviour component;

        void Awake() => component.enabled = true;
    }
}