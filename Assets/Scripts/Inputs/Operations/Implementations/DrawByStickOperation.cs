using Flux;
using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDrawByStickOperation", menuName = "Beau Tambour/Operations/Draw by Stick")]
    public class DrawByStickOperation : PhaseBoundOperation
    {
        private DrawingHandler drawingHandler;
        //
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            drawingHandler = Repository.GetSingle<DrawingHandler>(References.DrawingHandler);
        }

        protected override void RelayedOnStart(EventArgs inArgs) => HandleInput(inArgs);
        protected override void RelayedOnUpdate(EventArgs inArgs) => HandleInput(inArgs);
        protected override void RelayedOnEnd(EventArgs inArgs) => HandleInput(inArgs);

        private void HandleInput(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2EventArgs)) return;
            drawingHandler.Execute(vector2EventArgs.value);
        }
    }
}
