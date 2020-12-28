using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public class RuntimeCharacter : MonoBehaviour
    {
        [SerializeField] private Character asset;

        public Vector3 DialogueAnchor => dialogueAnchor.position;
        [Space, SerializeField] private Transform dialogueAnchor;

        public Vector3 TopCenter => topCenter.position;
        [SerializeField] private Transform topCenter;

        void Awake() => asset.Bootup(this);
    }
}