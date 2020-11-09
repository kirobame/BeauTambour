using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewContinuousVector2Handler", menuName = "Flux/Input/Handlers/Continuous Vector2")]
    public class ContinuousVector2Handler : ContinuousHandler<Vector2>
    {
        public override bool OnStarted(Vector2 input)
        {
            if (!base.OnStarted(input)) return false;
            
            Begin(new Vector2EventArgs(input));
            return true;
        }
        public override bool OnCanceled(Vector2 input)
        {
            if (!base.OnCanceled(input)) return false;
            
            End(new Vector2EventArgs(input));
            return true;
        }

        protected override void HandleInput(Vector2 input) => Prolong(new Vector2EventArgs(input));
    }
}