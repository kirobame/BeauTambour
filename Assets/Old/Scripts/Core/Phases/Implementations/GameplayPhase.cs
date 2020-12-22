using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class GameplayPhase : Phase
    {
        [SerializeField] private InputMapReference mapReference;

        public override void Begin()
        {
            base.Begin();
            mapReference.Value.Enable();
        }
        public override void End()
        {
            base.End();
            mapReference.Value.Disable();
        }
    }
}