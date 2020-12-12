using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewCompoundOperation", menuName = "Flux/Operations/Compound")]
    public class CompoundOperation : Operation
    {
        [SerializeField] private Operation parentOperation;
        [SerializeField] private Operation[] subOperations;

        protected Operation runtimeParentOperation => runtimeOperations[0];
        private Operation[] runtimeOperations;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            
            runtimeOperations = new Operation[subOperations.Length + 1];
            runtimeOperations[0] = Instantiate(parentOperation);
            runtimeParentOperation.Initialize(hook);

            for (var i = 1; i < runtimeOperations.Length; i++)
            {
                var runtimeSubOperation = Instantiate(subOperations[i - 1]);
                runtimeSubOperation.Initialize(hook);
                runtimeSubOperation.Bind(runtimeParentOperation);
                
                runtimeOperations[i] = runtimeSubOperation;
            }
        }

        public override void Bind(IBindable bindable) => runtimeParentOperation.Bind(bindable);
        public override void Unbind() => runtimeParentOperation.Unbind();
    }
}