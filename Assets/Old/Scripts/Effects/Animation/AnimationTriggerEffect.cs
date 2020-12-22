using Flux;
using UnityEngine;

namespace Deprecated
{
    public class AnimationTriggerEffect : AnimationEffect
    {
        [SerializeField] private new string name;

        protected override void Handle(Animator animator) => animator.SetTrigger(name);
    }
}