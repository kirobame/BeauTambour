using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSkipOperation", menuName = "Beau Tambour/Operations/Skip")]
    public class SkipOperation : SingleOperation
    {
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            Event.Open(OperationEvent.Skip);
        }

        public override void OnStart(EventArgs inArgs) => Event.Call(OperationEvent.Skip);
    }
}