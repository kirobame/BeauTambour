using System;
using System.Linq;
using UnityEngine;

namespace Flux
{
    public abstract class PreProcessedOperation<TOp, TPr> : SingleOperation where TOp : SingleOperation where TPr : PreProcessor<TOp>
    {
        [SerializeField] private TOp operation;
        [SerializeField] private TPr[] preProcessors;

        private TOp runtimeOperation;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            runtimeOperation = Instantiate(operation);
            runtimeOperation.Initialize(hook);
        }

        public override void OnStart(EventArgs inArgs)
        {
            foreach (var preProcessor in preProcessors) preProcessor.Affect(runtimeOperation);
            runtimeOperation.OnStart(inArgs);
            foreach (var preProcessor in preProcessors.Reverse()) preProcessor.Undo(runtimeOperation);
            
            Begin(inArgs);
        }
        public override void OnUpdate(EventArgs inArgs)
        {
            foreach (var preProcessor in preProcessors) preProcessor.Affect(runtimeOperation);
            runtimeOperation.OnUpdate(inArgs);
            foreach (var preProcessor in preProcessors.Reverse()) preProcessor.Undo(runtimeOperation);
            
            Prolong(inArgs);
        }
        public override void OnEnd(EventArgs inArgs)
        {
            foreach (var preProcessor in preProcessors) preProcessor.Affect(runtimeOperation);
            runtimeOperation.OnEnd(inArgs);
            foreach (var preProcessor in preProcessors.Reverse()) preProcessor.Undo(runtimeOperation);
            
            End(inArgs);
        }
    }
}