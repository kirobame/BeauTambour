using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerSelectionPhase : Phase
    {
        public override PhaseCategory Category => PhaseCategory.SpeakerSelection;

        public ISpeaker SelectedSpeaker => sortedSpeakers[SpeakerIndex];
        public int SpeakerIndex { get; private set; }

        private ISpeaker[] sortedSpeakers;

        protected override void Awake()
        {
            base.Awake();
            
            Event.Open<ISpeaker>(GameEvents.OnSpeakerSelected);
            Event.Open(GameEvents.OnSpeakerChoice);
            Event.Open(GameEvents.OnSpeakerChoiceDone);

            Event.Register(GameEvents.OnSpeakerChoiceDone, OnSpeakerChoiceDone);
        }

        public override void Begin()
        {
            base.Begin();
            
            SpeakerIndex = 0;
            sortedSpeakers = GameState.GetSortedSpeakers();
            
            Event.Call<ISpeaker>(GameEvents.OnSpeakerSelected, SelectedSpeaker);
        }
        public override void End()
        {
            base.End();
            GameState.Note.speaker = SelectedSpeaker;
        }

        public void SelectSpeaker(int index)
        {
            if (index < 0 || index >= sortedSpeakers.Length) return;

            SpeakerIndex = index;
            Event.Call<ISpeaker>(GameEvents.OnSpeakerSelected, SelectedSpeaker);
        }

        void OnSpeakerChoiceDone()
        {
            owner.Play(PhaseCategory.EmotionDrawing);
        }
    }
}