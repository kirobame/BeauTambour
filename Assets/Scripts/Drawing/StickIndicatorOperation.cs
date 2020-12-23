using Flux;
using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewCursorOperation", menuName = "Beau Tambour/Inputs/Operations/Cursor")]
    public class StickIndicatorOperation : SingleOperation
    {
        public enum EventType
        {
            OnUpdate,
            OnEnd
        }

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            Flux.Event.Open<Vector2>(EventType.OnUpdate);
            Flux.Event.Open<Vector2>(EventType.OnEnd);
        }

        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2EventArgs)) return;
            Flux.Event.Call(EventType.OnUpdate,vector2EventArgs.value);
        }
        public override void OnEnd(EventArgs inArgs)
        {
            Flux.Event.Call(EventType.OnEnd, Vector2.zero);
        } 
    }
}
