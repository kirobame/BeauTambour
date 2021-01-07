using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDrawByMouseOperation", menuName = "Beau Tambour/Operations/Draw by Mouse")]
    public class DrawByMouseOperation : PhaseBoundOperation
    {
        private DrawingHandler drawingHandler;

        private Vector2 previousInput;
        private bool canSendInput;
        
        private Vector2 screenCenter;
        private float radius;
        
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);

            canSendInput = true;
            Event.Register(GameEvents.OnStickUsed, () => canSendInput = false);
            
            drawingHandler = Repository.GetSingle<DrawingHandler>(References.DrawingHandler);
            var camera = Repository.GetSingle<Camera>(References.Camera);

            screenCenter = camera.WorldToScreenPoint(drawingHandler.transform.position);

            var offset = (Vector2)drawingHandler.transform.position + Vector2.right * drawingHandler.Radius;
            radius = Vector2.Distance(screenCenter, camera.WorldToScreenPoint(offset));
        }

        protected override void RelayedOnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2EventArgs)) return;
            
            if (!canSendInput)
            {
                if (vector2EventArgs.value != previousInput) canSendInput = true;
                else return;
            }
            
            var result = (vector2EventArgs.value - screenCenter) / radius;
            if (result.magnitude > 1) result.Normalize();
            
            //Debug.Log($"-------||-> SENDING MOUSE DRAW INPUT FROM {name} / {GetInstanceID()}");
            drawingHandler.Execute(result);
            previousInput = vector2EventArgs.value;
        }
    }
}