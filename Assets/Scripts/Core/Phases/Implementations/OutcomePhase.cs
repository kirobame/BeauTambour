using System.Collections.Generic;
using System.Text;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class OutcomePhase : Phase
    {
        #region Encapsualted Types

        public enum EventType
        {
            OnNoteCompleted,
            OnNoteCleared
        }
        #endregion
        
        public int NoteCount => notes.Count;
        public bool IsNoteBeingProcessed { get; private set; }
        
        [SerializeField] private InputMapReference mapReference;
        [SerializeField] private Encounter encounter;
        [SerializeField] private int partitionLength;

        [Space, SerializeField] private KeyMelodyRegistry keyMelodyRegistry;
        
        private List<Note> notes;
        private List<NoteAttribute> noteAttributes;

        protected override void Awake()
        {
            base.Awake();
            
            Event.Open<Note[]>(EventType.OnNoteCompleted);
            Event.Open(EventType.OnNoteCleared);
        }
        void Start()
        {
            encounter.BootUp();

            notes = new List<Note>();
            noteAttributes = new List<NoteAttribute>();

            ClearNotes();
        }
        
        public void EnqueueNoteAttribute(NoteAttribute noteAttribute)
        {
            if (!IsNoteBeingProcessed) return;
            this.noteAttributes.Add(noteAttribute);
        }
        public void EnqueueNoteAttributes(IEnumerable<NoteAttribute> noteAttributes)
        {
            if (!IsNoteBeingProcessed) return;
            this.noteAttributes.AddRange(noteAttributes);
        }
        
        public void BeginNote()
        {
            noteAttributes.Clear();
            IsNoteBeingProcessed = true;
        }

        public void CompleteNote()
        {
            if (!IsNoteBeingProcessed) return;

            var note = new Note(this.noteAttributes);
            if (NoteCount < partitionLength) notes.Add(note);
            else
            {
                for (var i = 1; i < partitionLength; i++) notes[i - 1] = notes[i];
                notes[partitionLength - 1] = note;
            }

            var emotion = string.Empty;
            var musician = string.Empty;

            foreach (var attribute in note.Attributes)
            {
                if (attribute is EmotionAttribute emotionAttribute) emotion = emotionAttribute.Emotion.ToString();
                else if (attribute is MusicianAttribute musicianAttribute) musician = musicianAttribute.Musician.name;
            }

            if (emotion != string.Empty && musician != string.Empty)
            {
                var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
                var audioSource = audioPool.RequestSingle();

                audioSource.clip = keyMelodyRegistry[$"{musician}-{emotion}"];
                audioSource.Play();
            }
            
            IsNoteBeingProcessed = false;
            Event.Call<Note[]>(EventType.OnNoteCompleted, notes.ToArray());
            
            var builder = new StringBuilder();
            builder.AppendLine($"Note [{notes.Count}] :");

            foreach (var attribute in notes[notes.Count - 1].Attributes) builder.AppendLine($"---{attribute}");
            Debug.Log(builder.ToString());
        }
        
        public void ClearNotes()
        {
            noteAttributes.Clear();
            IsNoteBeingProcessed = false;
            
            notes.Clear();
            Event.Call(EventType.OnNoteCleared);
        }

        public override void Begin()
        {
            base.Begin();
            mapReference.Value.Enable();
            
            encounter.Evaluate(notes.ToArray());
            ClearNotes();
        }
        public override void End()
        {
            base.End();
            mapReference.Value.Disable();
        }
    }
}