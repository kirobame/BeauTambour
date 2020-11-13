using UnityEngine;

namespace BeauTambour
{
    public class RuntimeCharacter : MonoBehaviour
    {
        public Transform DialoguePoint => dialoguePoint;
        [SerializeField] private Transform dialoguePoint;
    }
}