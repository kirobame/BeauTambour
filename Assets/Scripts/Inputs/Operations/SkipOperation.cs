using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSkipOperation", menuName = "Beau Tambour/Operations/Skip")]
    public class SkipOperation : SingleOperation
    {
        public override void Initialize(OperationHandler operationHandler)
        {
            base.Initialize(operationHandler);
            Event.Open(OperationEvent.Skip);
        }

        public override void OnStart(EventArgs inArgs)
        {
            Debug.Log("Skip");
            Event.Call(OperationEvent.Skip);
        }
    }
}