using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = Flux.Event;

namespace Deprecated
{
    public class Partition : MonoBehaviour
    {
        [SerializeField] private Transform cursor;

        [Space, SerializeField] private AnimationCurve moveCurve;
        [SerializeField] private AnimationCurve fadeCurve;
        
        [Space, SerializeField] private float gap;
        [SerializeField] private float startX;
        [SerializeField] private SpriteGroup[] noteSprites;

        [SerializeField] private EmotionColorRegistry emotionColorRegistry;
        [SerializeField] private MusicianIconRegistry musicianIconRegistry;

        private int advancement;

        private Coroutine moveCursorRoutine;
        private Coroutine[] noteAlphaRoutines;
        private Coroutine[] noteMoveRoutines;

        private Queue<int> activeGroups;
        private List<int> availableGroups;
        
        void Awake()
        {
            advancement = 0;
            
            Event.Register<Note[]>(OutcomePhase.EventType.OnNoteCompleted, OnNoteCompleted);
            Event.Register(OutcomePhase.EventType.OnNoteCleared, OnNoteCleared);
            
            noteAlphaRoutines = new Coroutine[noteSprites.Length];
            noteMoveRoutines = new Coroutine[noteSprites.Length];
            
            activeGroups = new Queue<int>();
            availableGroups = new List<int>();
            for (var i = 0; i < noteSprites.Length; i++) availableGroups.Add(i);
        }

        private void OnNoteCompleted(Note[] notes) => StartCoroutine(NoteCompletionRoutine(notes));
        private IEnumerator NoteCompletionRoutine(Note[] notes)
        {
            if (advancement >= 2)
            {
                foreach (var activeGroup in activeGroups)
                {
                    var noteSprite = noteSprites[activeGroup];

                    if (noteMoveRoutines[activeGroup] != null) StopCoroutine(noteMoveRoutines[activeGroup]);
                    noteMoveRoutines[activeGroup] =
                        StartCoroutine(MoveNoteRoutine(noteMoveRoutines[activeGroup], noteSprite, 0.6f));
                }

                yield return new WaitForSeconds(0.6f);

                var group = activeGroups.Dequeue();
                noteSprites[group].SetAlpha(0);

                availableGroups.Add(group);
            }
            else
            {
                if (moveCursorRoutine != null) StopCoroutine(MoveCursorRoutine());
                moveCursorRoutine = StartCoroutine(MoveCursorRoutine());
            }

            var usedGroup = availableGroups.First();
            availableGroups.RemoveAt(0);
            
            activeGroups.Enqueue(usedGroup);
            
            var musicianAttribute = (MusicianAttribute)notes.Last().Attributes.First(attribute => attribute is MusicianAttribute);
            var emotionAttribute = (EmotionAttribute)notes.Last().Attributes.First(attribute => attribute is EmotionAttribute);

            var color = emotionColorRegistry[emotionAttribute.Emotion];
            color.a = noteSprites[usedGroup][0].color.a;
            noteSprites[usedGroup][0].color = color;
            
            var musician = musicianAttribute.Musician;
            noteSprites[usedGroup][1].sprite = musicianIconRegistry[musician];
            
            var localPosition = noteSprites[usedGroup].transform.localPosition;
            localPosition.x = startX + gap * Mathf.Clamp(advancement, 0, 1);

            noteSprites[usedGroup].transform.localPosition = localPosition;
            
            if (noteAlphaRoutines[usedGroup] != null) StopCoroutine(noteAlphaRoutines[usedGroup]);
            noteAlphaRoutines[usedGroup] = StartCoroutine(NoteAlphaRoutine(noteAlphaRoutines[usedGroup], noteSprites[usedGroup], 1f));

            advancement++;
        }
        
        private void OnNoteCleared()
        {
            if (advancement == 0) return;
            advancement = 0;
            
            if( moveCursorRoutine != null) StopCoroutine(moveCursorRoutine);
            moveCursorRoutine = StartCoroutine(MoveCursorRoutine());

            while (activeGroups.Count > 0)
            {
                var group = activeGroups.Dequeue();
                
                if (noteAlphaRoutines[group] != null) StopCoroutine(noteAlphaRoutines[group]);
                noteAlphaRoutines[group] = StartCoroutine(NoteAlphaRoutine(noteAlphaRoutines[group], noteSprites[group], 0f));
                
                availableGroups.Add(group);
            }
        }

        private IEnumerator MoveCursorRoutine()
        {
            var start = cursor.localPosition.x;
            var end = startX + gap * Mathf.Clamp(advancement, 0, 1);

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
                localPosition.x = Mathf.Lerp(start, end, moveCurve.Evaluate(ratio));

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
        private IEnumerator MoveNoteRoutine(Coroutine routine, SpriteGroup group, float goal)
        {
            var start = group[0].transform.position.x;
            var end = start - gap;
            
            var time = 0f;
            while (time < goal)
            {
                Execute(Mathf.Clamp01(time / goal));
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }

            Execute(1f);
            void Execute(float ratio)
            {
                var position = group.transform.position;
                position.x = Mathf.Lerp(start, end, moveCurve.Evaluate(ratio));

                group.transform.position = position;
            }

            routine = null;
        }
    }
}