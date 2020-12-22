using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private Musician musician;
        [SerializeField, ContextMenuItem("Display", "Display")] private Emotion emotion;

        public void Display()
        {
            GameState.Note.musician = musician;
            GameState.Note.emotion = emotion;
            
            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            phaseHandler.Play(PhaseCategory.Dialogue);
        }
    }
}