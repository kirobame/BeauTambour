using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class RuntimeCharacter : MonoBehaviour
    {
        public Transform DialoguePoint => dialoguePoint;
        [SerializeField] private Transform dialoguePoint;

        public Transform VisualEffectPoint => visualEffectPoint;
        [SerializeField] private Transform visualEffectPoint;
        [SerializeField] private Animator animator;
        
        [Space, SerializeField] private KeyMelodyRegistry keyMelodyRegistry;
        private PoolableAudio pooledMusic;

        public void PlayMusic(Note note)
        {
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
                pooledMusic = audioPool.RequestSinglePoolable();

                pooledMusic.OnDone += OnMusicEnd;
                
                pooledMusic.Value.clip = keyMelodyRegistry[$"{musician}-{emotion}"];
                pooledMusic.Value.Play();
            }
            
            animator.SetBool("IsPlaying", true);
        }
        void OnMusicEnd()
        {
            pooledMusic.OnDone -= OnMusicEnd;
            animator.SetBool("IsPlaying", false);
        }

        public void BeginTalking() => animator.SetBool("IsTalking", true);
        public void StopTalking() => animator.SetBool("IsTalking", false);
    }
}