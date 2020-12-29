using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using System;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewButtonHandler", menuName = "Beau Tambour/Inputs/Handlers/Button")]
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