using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewPauseOperation", menuName = "Beau Tambour/Operations/Pause")]
    public class PauseOperation : SingleOperation
    {
        private bool state;
        //
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);

            Event.Register(PauseMenu.EventType.OnUnpause, () => state = false);
            Event.Register(PauseMenu.EventType.OnPause, () => state = true);
        }

        public override void OnStart(EventArgs inArgs)
        {
            if (state) Event.Call(PauseMenu.EventType.OnUnpause);
            else Event.Call(PauseMenu.EventType.OnPause);
        }
    }
}