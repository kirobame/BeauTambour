using System;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDeleteNoteOperation", menuName = "Beau Tambour/Operations/Delete Note")]
    public class DeleteNoteOperation : PhaseBoundOperation
    {
        protected override void RelayedOnStart(EventArgs inArgs)
        {
            if (GameState.validationMade) return;
            
            Event.Call(GameEvents.OnNoteDeletion);
            GameState.validationMade = true;
        }
    }
}