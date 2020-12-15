using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewClearShapeOperation", menuName = "Beau Tambour/Operations/Clear Shape")]
    public class ClearShapeOperation : SingleOperation
    {
        private bool canClear;
        
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            
            Event.Register(DrawOperation.EventType.OnStart, () => canClear = true);
            Event.Register(DrawOperation.EventType.OnShapeEnd, () => canClear = false);
            Event.Register(DeviceAlternationHandler.EventType.OnChange, Execute);
        }

        public override void OnStart(EventArgs inArgs) => Execute();

        private void Execute()
        {
            var drawOperations = Repository.GetAll<DrawOperation>(Reference.DrawOperation);
            foreach (var drawOperation in drawOperations) drawOperation.End(false);
        }
    }
}