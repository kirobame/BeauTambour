using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerSelectionPhase : Phase
    {
        [SerializeField] private InputMapReference inputMap;

        public override PhaseCategory Category => PhaseCategory.SpeakerSelection;

        public Character SelectedSpeaker { get; private set; }
        public int SpeakerIndex { get; private set; }

        private Character[] sortedSpeakers;

        protected override void Awake()
        {
            base.Awake();
            
            Event.Open<Character, int>(GameEvents.OnSpeakerSelected);
            Event.Open(GameEvents.OnSpeakerChoice);
            Event.Open(GameEvents.OnSpeakerChoiceDone);

            Event.Register(GameEvents.OnSpeakerChoiceDone, OnSpeakerChoiceDone);
            Event.Register(GameEvents.OnSpeakerChoice, OnSpeakerChoice);

            Event.Open(GameEvents.OnInterlocutorConvinced);
            Event.Register(GameEvents.OnInterlocutorConvinced, OnInterlocutorConvinced);
        }

        public override void Begin()
        {
            base.Begin();
            inputMap.Value.Enable();

            SpeakerIndex = 0;
            sortedSpeakers = GameState.GetSortedSpeakers();
            
            for (var i = 0; i < sortedSpeakers.Length; i++)
            {
                if (sortedSpeakers[i] == SelectedSpeaker)
                {
                    SpeakerIndex = i;
                    break;
                }
            }

            SelectedSpeaker = sortedSpeakers[SpeakerIndex];
            var code = SpeakerIndex == 0 ? 1 : SpeakerIndex == sortedSpeakers.Length - 1 ? 2 : 0;
            Event.Call<Character, int>(GameEvents.OnSpeakerSelected, SelectedSpeaker, code);
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
            SelectedSpeaker = sortedSpeakers[SpeakerIndex];
            
            var code = SpeakerIndex == 0 ? 1 : SpeakerIndex == sortedSpeakers.Length - 1 ? 2 : 0;
            Event.Call<Character, int>(GameEvents.OnSpeakerSelected, SelectedSpeaker, code);
        }

        void OnSpeakerChoiceDone() => owner.Play(PhaseCategory.EmotionDrawing);
        void OnSpeakerChoice() => inputMap.Value.Disable();

        void OnInterlocutorConvinced()
        {
            var encounter = Repository.GetSingle<Encounter>(References.Encounter);
            SelectedSpeaker = encounter.Interlocutor;
        }
    }
}