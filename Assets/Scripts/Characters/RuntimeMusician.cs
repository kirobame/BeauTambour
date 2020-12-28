using UnityEngine;

namespace BeauTambour
{
    public class RuntimeMusician : RuntimeCharacter
    {
        [SerializeField] private Animator animator;

        public void BeginTalking() => animator.SetBool("IsTalking", true);
        public void StopTalking() => animator.SetBool("IsTalking", false);
    }
}