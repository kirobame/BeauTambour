using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewCancelDrawingOperation", menuName = "Beau Tambour/Operations/Cancel Drawing")]
    public class CancelDrawingOperation : PhaseBoundOperation
    {
        protected override void RelayedOnStart(EventArgs inArgs)
        {
            var drawingHandler = Repository.GetSingle<DrawingHandler>(References.DrawingHandler);
            drawingHandler.TryCancelDrawing();
        }
    }
}