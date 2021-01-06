using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class AnimationHelper : MonoBehaviour
    {
        private enum Trigger
        {
            Joie,
            Anticipation,
            Colere,
            Confiance,
            Degout,
            Peur,
            Tristesse,
            Surprise
        }
        
        [ContextMenuItem("Execute", "Execute")]
        [SerializeField] private RuntimeCharacter character;

        [ContextMenuItem("Execute", "Execute")]
        [SerializeField] private PoolableAnimation vfx;
        
        [ContextMenuItem("Execute", "Execute")]
        [Space, SerializeField] private Animator animator;

        [ContextMenuItem("Execute", "Execute")] 
        [SerializeField] private Trigger trigger;

        public void Execute()
        {
            var vfxPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
            var poolableVfx = vfxPool.RequestSingle(vfx);

            poolableVfx.transform.parent = character.HeadSocket.Attach;
            poolableVfx.transform.localPosition = Vector3.zero;
            
            poolableVfx.SetTrigger("Play");
            animator.SetTrigger($"TriggerEmote{trigger}");
        }
    }
}