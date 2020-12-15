using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "newVisualEffectSignal", menuName = "Beau Tambour/Signals/Visual Effect")]
    public class VisualEffectSignal : Signal
    {
        public override string Category => "vfx";

        [SerializeField] private PoolableVisualEffect poolable;
        private PoolableVisualEffect runtimePoolable;

        public override void Execute(MonoBehaviour hook, Character character, string[] args)
        {
            var visualEffectPool = Repository.GetSingle<VisualEffectPool>(Pool.VisualEffect);

            runtimePoolable = visualEffectPool.RequestSinglePoolable(poolable);

            runtimePoolable.transform.position = character.Instance.VisualEffectPoint.position;
            runtimePoolable.Value.SetTrigger("Play");

            runtimePoolable.OnDone += OnPoolableDeactivation;
        }

        void OnPoolableDeactivation()
        {
            runtimePoolable.OnDone -= OnPoolableDeactivation;
            End();
        }
    }
}