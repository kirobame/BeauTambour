using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewVector2ButtonHandler", menuName = "Beau Tambour/Inputs/Handlers/Vector2 Button")]
    public class Vector2ButtonHandler : InputHandler<Vector2>
    {
        public override bool OnStarted(Vector2 input)
        {
            if (!base.OnStarted(input)) return false;
            
            Begin(new Vector2EventArgs(input));
            return true;
        }
    }
}