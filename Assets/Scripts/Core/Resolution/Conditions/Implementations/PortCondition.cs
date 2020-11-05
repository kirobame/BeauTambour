using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("Is Port Valid")]
    public class PortCondition : Condition
    {
        [SerializeField] private OutcomePort port;

        public override bool IsMet(Note[] notes) => port.IsValid;
    }
}