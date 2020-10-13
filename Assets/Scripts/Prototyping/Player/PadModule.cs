using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class PadModule : Module<bool>
    {
        [SerializeField] private int direction;
        [SerializeField] private ShiftAction shiftAction;
        
        protected override void OnActionStarted(bool input)
        {
            shiftAction.direction = direction;
            shiftAction.TryBeginExecution();
        }

        protected override void OnAction(bool input) { }
        protected override void OnActionEnded(bool input) { }
    }
}