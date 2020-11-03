using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewStickActivator", menuName = "Beau Tambour/Activators/Stick")]
    public class StickActivator : ContinuousHandler<Vector2>
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