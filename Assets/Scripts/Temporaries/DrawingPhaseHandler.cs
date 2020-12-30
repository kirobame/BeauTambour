using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class DrawingPhaseHandler : MonoBehaviour
    {
        [ContextMenuItem("Begin", "Begin")]
        [SerializeField] private int number;
        
        public void Begin()
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            phaseHandler.Play(PhaseCategory.EmotionDrawing);
        }
    }
}