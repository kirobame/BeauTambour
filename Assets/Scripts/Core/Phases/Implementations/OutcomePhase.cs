﻿using System.Collections.Generic;
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
            
            if (NoteCount < partitionLength) notes.Add(new Note(this.noteAttributes));
            else
            {
                for (var i = 1; i < partitionLength; i++) notes[i - 1] = notes[i];
                notes[partitionLength - 1] = new Note(this.noteAttributes);
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