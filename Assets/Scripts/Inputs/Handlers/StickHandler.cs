using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewStickHandler", menuName = "Beau Tambour/Handlers/Stick")]
    public class StickHandler : ContinuousHandler<Vector2>
    {
        protected override void OnStart(Vector2 input)
        {
            base.OnStart(input);
            Begin(new Vector2EventArgs(input));
        }
        protected override void OnCanceled(Vector2 input)
        {
            base.OnCanceled(input);
            End(new Vector2EventArgs(input));
        }

        protected override void HandleInput(Vector2 input) => Prolong(new Vector2EventArgs(input));
    }
}