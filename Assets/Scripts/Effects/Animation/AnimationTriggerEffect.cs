using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Animation/Trigger")]
    [ItemName("Animation Trigger")]
    public class AnimationTriggerEffect : AnimationEffect
    {
        [SerializeField] private new string name;

        protected override void Handle(Animator animator) => animator.SetTrigger(name);
    }
}