using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using Event = Flux.Event;

namespace BeauTambour
{
    public class MusicianIndicator : MonoBehaviour
    {
        [SerializeField] private MusicianIconRegistry icons;
        [SerializeField] private SpriteRenderer musicianIndicator;

        private void Start()
        {
            Event.Register($"{PhaseCategory.EmotionDrawing}.{PhaseCallback.Start}",SetMusicianSprite);
        }

        public void SetMusicianSprite()
        {
            Sprite sprite;
            if(icons.TryGet(GameState.Note.speaker.Actor, out sprite))
            {
                musicianIndicator.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"{GameState.Note.speaker.Actor}'s icon not found in registry.");
            }
        }
    }
}