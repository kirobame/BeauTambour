using System.Collections;
using UnityEngine;

namespace Orion
{
    public class AnimatorEffect : Feedback
    {
        [SerializeField] private IReadable<Animator> animatorProxy;
        private Animator animator => animatorProxy.Read();
        [SerializeField] private IAnimatorAffect affect;
        
        public override IEnumerator GetRoutine()
        {
            affect.Play(animator);
            yield break;
        }
    }
}