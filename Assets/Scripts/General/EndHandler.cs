using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class EndHandler : MonoBehaviour
    {
        void Awake() => Event.Register(GameEvents.OnEncounterEnd, Execute);

        private void Execute()
        {
            
        }
    }
}