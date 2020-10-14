using Orion;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BeauTambour.Prototyping
{
    public class PhaseUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image icon;
        [SerializeField] private PhaseRegistry phaseRegistry;

        private Phase previousPhase;
        
        void Start()
        {
            var roundHandler = Repository.Get<RoundHandler>();
            roundHandler.OnPhaseChange += OnPhaseChange;

            OnPhaseChange(roundHandler.Phases.First());
        }

        private void OnPhaseChange(IPhase phase)
        {
            if (!phaseRegistry.HasKey(phase.Type)) return;
            
            text.text = phase.Type.ToString();
            icon.sprite = phaseRegistry[phase.Type];
        }
    }
}