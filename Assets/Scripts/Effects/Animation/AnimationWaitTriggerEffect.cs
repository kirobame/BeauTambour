using Flux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Animation/WaitTrigger")]
    [ItemName("Animation Wait Trigger")]
    public class AnimationWaitTriggerEffect : Effect
    {
        [SerializeField] private Animator target;
        [SerializeField] private new string name;

        private bool isHandled = false;

        public override void Initialize()
        {
            base.Initialize();
            isHandled = false;
        }
        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            if (!isHandled)
            {
                Handle(target);
                isHandled = true;
            }
            if (target.GetCurrentAnimatorStateInfo(0).IsName("Void"))
            {
                return base.Evaluate(advancement, registry, deltaTime, out prolong);
            }
            prolong = false;
            return advancement;
        }

        protected void Handle(Animator animator)
        {
            animator.SetTrigger(name);
        }
    }
}
