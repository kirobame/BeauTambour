using System.Collections;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "newAnimationSignal", menuName = "Beau Tambour/Signals/Animation")]
    public class AnimationSignal : Signal
    {
        public override string Category => "animation";

        public override void Execute(MonoBehaviour hook, Character speaker, string[] args) => hook.StartCoroutine(WaitRoutine(speaker));
        private IEnumerator WaitRoutine(Character speaker)
        {
            //Debug.Log($"{Time.time} -- [Animation]:[{speaker.Actor}]:[{Key}]");

            if (speaker.RuntimeLink.ActOut(Key))
            {
                var animator = speaker.RuntimeLink.Animator;
                
                var layerIndex = animator.GetLayerIndex("Actions");
                var state = animator.GetCurrentAnimatorStateInfo(layerIndex);

                while (!state.IsTag(Key.ToString()))
                {
                    yield return new WaitForEndOfFrame();
                    state = animator.GetCurrentAnimatorStateInfo(layerIndex);
                }
            
                while (state.IsTag(Key.ToString()))
                {
                    yield return new WaitForEndOfFrame();
                    state = animator.GetCurrentAnimatorStateInfo(layerIndex);
                }
            }
            
            yield return new WaitForSeconds(0.4f);
            End();
        }
        
        private IEnumerator DelayedDeactivationRoutine()
        {
            yield return  new WaitForEndOfFrame();
            End();
        }
    }
}