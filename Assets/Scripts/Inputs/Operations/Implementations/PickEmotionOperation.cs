using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using System;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewPickEmotionOperation", menuName = "Beau Tambour/Operations/Pick Emotion")]
    public class PickEmotionOperation : PhaseBoundOperation
    {
        protected override void RelayedOnStart(EventArgs inArgs)
        {
            var drawingHandler = Repository.GetSingle<DrawingHandler>(References.DrawingHandler);
            drawingHandler.TryBeginDrawing();
        }
    }
}