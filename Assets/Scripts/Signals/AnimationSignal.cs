using System.Collections;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "newAnimationSignal", menuName = "Beau Tambour/Signals/Animation")]
    public class AnimationSignal : Signal
    {
        public override string Category => "animation";

        public override void Execute(MonoBehaviour hook, ISpeaker speaker, string[] args)
        {
            if (speaker is Interlocutor) hook.StartCoroutine(DelayedDeactivationRoutine());
            else hook.StartCoroutine(WaitRoutine(speaker));
        }
        
        private IEnumerator DelayedDeactivationRoutine()
        {
            yield return  new WaitForEndOfFrame();
            End();
        }
        private IEnumerator WaitRoutine(ISpeaker speaker)
        {
            Debug.Log($"{Time.time} -- [Animation]:[{speaker.Actor}]:[{Key}]");
            if (speaker.Animator == null)
            {
                yield return new WaitForSeconds(0.4f);
                End();
            }
            else
            {
                speaker.ActOut(Key);

                var layerIndex = speaker.Animator.GetLayerIndex("Actions");
                var state = speaker.Animator.GetCurrentAnimatorStateInfo(layerIndex);

                while (!state.IsTag(Key.ToString()))
                {
                    yield return new WaitForEndOfFrame();
                    state = speaker.Animator.GetCurrentAnimatorStateInfo(layerIndex);
                }
            
                while (state.IsTag(Key.ToString()))
                {
                    yield return new WaitForEndOfFrame();
                    state = speaker.Animator.GetCurrentAnimatorStateInfo(layerIndex);
                }
            
                yield return new WaitForSeconds(0.4f);
                End();
            }
        }
    }
}