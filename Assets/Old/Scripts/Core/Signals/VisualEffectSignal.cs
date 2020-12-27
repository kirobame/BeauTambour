using BeauTambour;
using Flux;
using UnityEngine;
using UnityEngine.Audio;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "newVisualEffectSignal", menuName = "Beau Tambour/Signals/Visual Effect")]
   /*public class VisualEffectSignal : Signal
    {
        public override string Category => "vfx";

        [SerializeField] private PoolableVisualEffect poolable;
        private PoolableVisualEffect runtimePoolable;

        public override void Execute(MonoBehaviour hook, Character character, string[] args)
        {
            var visualEffectPool = Repository.GetSingle<VisualEffectPool>(Pool.VisualEffect);
            runtimePoolable = visualEffectPool.RequestSinglePoolable(poolable);
            
            var localScale = runtimePoolable.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            
            if (character is Interlocutor) localScale.x *= -1;
            else localScale.x *= 1;

            runtimePoolable.transform.localScale = localScale;
            runtimePoolable.transform.position = character.Instance.VisualEffectPoint.position;

            runtimePoolable.Value.SetTrigger("Play");
            runtimePoolable.OnDone += OnPoolableDeactivation;
        }

        void OnPoolableDeactivation()
        {
            runtimePoolable.OnDone -= OnPoolableDeactivation;
            End();
        }
    }*/
}