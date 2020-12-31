using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public abstract class PhaseBoundOperation : SingleOperation
    {
        [SerializeField] protected PhaseCategory phase;
        protected PhaseHandler phaseHandler;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
        }

        public sealed override void OnStart(EventArgs inArgs)
        {
            base.OnStart(inArgs);
            
            if (phase != PhaseCategory.None && phaseHandler.CurrentCategory != phase) return;
            RelayedOnStart(inArgs);
        }
        protected virtual void RelayedOnStart(EventArgs inArgs) { }

        public sealed override void OnUpdate(EventArgs inArgs)
        {
            base.OnUpdate(inArgs);
            
            if (phase != PhaseCategory.None && phaseHandler.CurrentCategory != phase) return;
            RelayedOnUpdate(inArgs);
        }
        protected virtual void RelayedOnUpdate(EventArgs inArgs) { }

        public sealed override void OnEnd(EventArgs inArgs)
        {
            base.OnEnd(inArgs);
            
            if (phase != PhaseCategory.None && phaseHandler.CurrentCategory != phase) return;
            RelayedOnEnd(inArgs);
        }
        protected virtual void RelayedOnEnd(EventArgs inArgs) { }
    }
}