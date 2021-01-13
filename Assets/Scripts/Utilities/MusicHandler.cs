using System.Collections;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class MusicHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip clip;
        [SerializeField] private Vector2 volumeRange;

        [Space, SerializeField] private AnimationCurve inCurve;
        [SerializeField] private AnimationCurve outCurve;

        private float startingVolume;
        
        void Awake()
        {
            Repository.Reference(this, References.MusicHandler);
            source.volume = volumeRange.y;

            Event.Register(GameEvents.OnEncounterBootedUp, OnEncounterBootedUp);
        }

        void OnEncounterBootedUp()
        {
            source.clip = clip;
            source.volume = volumeRange.x;
            
            Prepare();
            source.Play();

            StartCoroutine(BootupRoutine());
        }
        private IEnumerator BootupRoutine()
        {
            var time = 0.0f;
            var goal = 0.75f;

            while (time < goal)
            {
                In(time / goal);
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            In(1.0f);
        }

        public void Prepare() => startingVolume = source.volume;

        public void In(float ratio) => source.volume = Mathf.Lerp(startingVolume, volumeRange.y, inCurve.Evaluate(ratio));
        public void Out(float ratio) => source.volume = Mathf.Lerp(startingVolume, volumeRange.x, outCurve.Evaluate(ratio));
    }
}