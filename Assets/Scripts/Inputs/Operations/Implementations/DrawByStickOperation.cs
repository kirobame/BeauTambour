using Flux;
using System;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDrawByStickOperation", menuName = "Beau Tambour/Operations/Draw by Stick")]
    public class DrawByStickOperation : PhaseBoundOperation
    {
        private DrawingHandler drawingHandler;
        
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);

            Event.Open(GameEvents.OnStickUsed);
            drawingHandler = Repository.GetSingle<DrawingHandler>(References.DrawingHandler);
        }

        protected override void RelayedOnStart(EventArgs inArgs)
        {
            Event.Call(GameEvents.OnStickUsed);
            HandleInput(inArgs);
        }
        protected override void RelayedOnUpdate(EventArgs inArgs) => HandleInput(inArgs);
        protected override void RelayedOnEnd(EventArgs inArgs) => HandleInput(inArgs);

        private void HandleInput(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2EventArgs)) return;
            
            //Debug.Log($"-------||-> SENDING STICK DRAW INPUT FROM {name} / {GetInstanceID()}");
            drawingHandler.Execute(vector2EventArgs.value);
        }
    }
}
