using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public abstract class RythmOperation : SingleOperation
    {
        [SerializeField] private double marginTolerance;
        
        public override void OnStart(EventArgs inArgs)
        {
            if (!TryGetAction(out var action)) return;
            
            var tolerance = 0d;
            if (marginTolerance > 0d) tolerance = marginTolerance;
            else
            {
                var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);
                tolerance = settings.RythmMarginTolerance;
            }
            
            var rythmHandler = Repository.GetSingle<RythmHandler>(Reference.RythmHandler);
            
            if (rythmHandler.TryEnqueue(action, tolerance)) OnEnqueueSuccess();
            else OnEnqueueFailure();
        }

        protected abstract bool TryGetAction(out IRythmQueueable action);

        protected virtual void OnEnqueueSuccess() { }
        protected virtual void OnEnqueueFailure() { }
    }
}