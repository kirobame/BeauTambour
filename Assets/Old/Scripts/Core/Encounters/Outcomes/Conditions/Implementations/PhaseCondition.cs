using System.Linq;
using Flux;
using UnityEngine;

namespace Deprecated
{
    //[ItemPath("Is In Phase")]
    //[ItemName("Is In Phase")]
    public class PhaseCondition : Condition
    {
        protected string[] ids => joinedIds.Split('/');
        [SerializeField] private string joinedIds;

        public override bool IsMet(Encounter encounter, Note[] notes)
        {
            return ids.Any(id => encounter.Interlocutor.CurrentBlock.Id == id);
        }
    }
}