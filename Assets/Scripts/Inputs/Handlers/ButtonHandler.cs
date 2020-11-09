using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewButtonHandler", menuName = "Beau Tambour/Handlers/Button")]
    public class ButtonHandler : InputHandler<bool>
    {
        public override bool OnPerformed(bool input)
        {
            if (!base.OnPerformed(input)) return false;
            
            Begin(new EventArgs());
            return true;
        }
    }
}