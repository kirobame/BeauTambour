using System.Linq;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class ArcProgress : MonoBehaviour
    {
        [SerializeField] private ArcSegment[] segments;

        private int speakerCount;
        
        void Awake()
        {
            Event.Register(GameEvents.OnBlockPassed, OnBlockPassed);
            
            Event.Open<Musician>(GameEvents.OnMusicianArcCompleted);
            Event.Register<Musician>(GameEvents.OnMusicianArcCompleted, OnMusicianArcCompleted);
        }

        void OnBlockPassed()
        {
            speakerCount = GameState.Speakers.Count() - 1;
            for (var i = 0; i < speakerCount; i++)
            {
                segments[i].gameObject.SetActive(true);
                segments[i].Reboot();
            }

            for (var i = speakerCount; i < segments.Length; i++) segments[i].gameObject.SetActive(false);
        }
        void OnMusicianArcCompleted(Musician musician)
        {
            var index = speakerCount - 1 + GameState.FinishedArcsCount;
            segments[index].Complete(musician);
        }
    }
}