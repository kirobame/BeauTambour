using UnityEngine;

namespace BeauTambour
{
    public class RuntimeInterlocutor : RuntimeCharacter
    {
        [SerializeField] private Animator animator;

        public void BeginTalking() => animator.SetBool("IsTalking", true);
        public void StopTalking() => animator.SetBool("IsTalking", false);
    }
}