using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewStickHandler", menuName = "Beau Tambour/Inputs/Handlers/Stick")]
    public class StickHandler : ContinuousHandler<Vector2>
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

        protected override void HandleInput(Vector2 input)
        {
            Prolong(new Vector2EventArgs(input));
        }
    }
}