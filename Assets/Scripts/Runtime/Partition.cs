using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Partition : MonoBehaviour
    {
        [SerializeField] private Transform cursor;
        
        [Space, SerializeField] private AnimationCurve moveCurve;
        [SerializeField] private AnimationCurve fadeCurve;
        
        [SerializeField] private SpriteGroup[] noteSprites;
        
        [SerializeField] private EmotionColorRegistry emotionColorRegistry;
        [SerializeField] private MusicianIconRegistry musicianIconRegistry;

        private int advancement;
        
        private Coroutine moveCursorRoutine;
        private Coroutine[] noteAlphaRoutines;
        
        void Awake()
        {
            Event.Register<Note[]>(OutcomePhase.EventType.OnNoteCompleted, OnNoteCompleted);
            Event.Register(OutcomePhase.EventType.OnNoteCleared, OnNoteCleared);
            
            noteAlphaRoutines = new Coroutine[noteSprites.Length];
        }

        private void OnNoteCompleted(Note[] notes)
        {
            for (var i = 0; i < notes.Length; i++)
            {
                var attribute = notes[i].Attributes.First(item => item is EmotionAttribute);
                if (attribute != null)
                {
                    var emotion = ((EmotionAttribute) attribute).Emotion;
                    
                    var color = emotionColorRegistry[emotion];
                    color.a = noteSprites[i][0].color.a;

                    noteSprites[i][0].color = color;
                }
                
                attribute = notes[i].Attributes.First(item => item is MusicianAttribute);
                if (attribute != null)
                {
                    var musician = ((MusicianAttribute)attribute).Musician;
                    noteSprites[i][1].sprite = musicianIconRegistry[musician];
                }
            }

            if (advancement == notes.Length) return;
            advancement = notes.Length;
            
            var index = notes.Length - 1;
            cursor.parent = noteSprites[index].transform;
            
            if (moveCursorRoutine != null) StopCoroutine(moveCursorRoutine);
            moveCursorRoutine = StartCoroutine(MoveCursorRoutine());
            
            if (noteAlphaRoutines[index] != null) StopCoroutine(noteAlphaRoutines[index]);
            noteAlphaRoutines[index] = StartCoroutine(NoteAlphaRoutine(noteAlphaRoutines[index], noteSprites[index], 1f));
        }
        private void OnNoteCleared()
        {
            if (advancement == 0) return;
            advancement = 0;
            
            cursor.parent = noteSprites[0].transform;
            
            if( moveCursorRoutine != null) StopCoroutine(moveCursorRoutine);
            moveCursorRoutine = StartCoroutine(MoveCursorRoutine());

            for (var i = 0; i < noteSprites.Length; i++)
            {
                if (noteAlphaRoutines[i] != null) StopCoroutine(noteAlphaRoutines[i]);
                noteAlphaRoutines[i] = StartCoroutine(NoteAlphaRoutine(noteAlphaRoutines[i], noteSprites[i], 0f));
            }
        }

        private IEnumerator MoveCursorRoutine()
        {
            var start = cursor.localPosition.x;

            var time = 0f;
            var goal = 0.6f;
            
            while (time < goal)
            {
                Execute(Mathf.Clamp01(time / goal));
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            
            Execute(1f);
            void Execute(float ratio)
            {
                var localPosition = cursor.localPosition;
                localPosition.x = Mathf.Lerp(start, 0f, moveCurve.Evaluate(ratio));

                cursor.localPosition = localPosition;
            }

            moveCursorRoutine = null;
        }
        private IEnumerator NoteAlphaRoutine(Coroutine routine, SpriteGroup group, float alphaGoal)
        {
            var start = group.StartingAlpha;
            
            var time = 0f;
            var goal = 0.6f;
            
            while (time < goal)
            {
                Execute(Mathf.Clamp01(time / goal));
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }

            Execute(1f);
            void Execute(float ratio) => group.SetAlpha(Mathf.Lerp(start, alphaGoal, fadeCurve.Evaluate(ratio)));

            routine = null;
        }
    }
}