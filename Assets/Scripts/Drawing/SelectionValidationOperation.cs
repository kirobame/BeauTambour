using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using System;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewValidationOperation", menuName = "Beau Tambour/Inputs/Operations/Validation")]
    public class SelectionValidationOperation : SingleOperation
    {
        public enum EventType
        {
            OnStart,
        }

        private bool canValidate;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            canValidate = false;
            Event.Open(EventType.OnStart);

            Event.Register(StickIndicatorBehavior.EventType.OnMagnetize, () => canValidate = true);
            Event.Register(StickIndicatorBehavior.EventType.OnUnMagnetize, () => canValidate = false);
        }

        public override void OnStart(EventArgs inArgs)
        {
            base.OnStart(inArgs);
            if (canValidate)
            {
                Event.Call(EventType.OnStart);
            }
        }
    }
}