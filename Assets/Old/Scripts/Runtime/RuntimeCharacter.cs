using System.Collections;
using BeauTambour;
using Flux;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Flux.Event;

namespace Deprecated
{
    public class RuntimeCharacter : MonoBehaviour
    {
        [SerializeField] private Character character;
        
        public Transform DialoguePoint => dialoguePoint;
        [Space, SerializeField] private Transform dialoguePoint;

        public Transform VisualEffectPoint => visualEffectPoint;
        [SerializeField] private Transform visualEffectPoint;
        [SerializeField] private Animator animator;
        
        [Space, SerializeField] private KeyMelodyRegistry keyMelodyRegistry;
        [SerializeField, Range(0,1)] private float volume;

        [Space, SerializeField] private SkeletonMecanim skeleton;
        [SerializeField] private Animator selectionVfx;
        [SerializeField] private Animator musicVfx;

        private Vector2 musicVfxOffset;
        private Bone instrumentBone;
        
        private PoolableAudio pooledMusic;
        private bool isSelected;
        private bool isPlaying;

        void Awake()
        {
            Event.Register<Musician>(TempEvent.OnMusicianPickedExtended, musician =>
            {
                if (musician == character && !isSelected)
                {
                    selectionVfx.gameObject.SetActive(true);
                    selectionVfx.SetTrigger("In");

                    isSelected = true;
                }
                else if (musician != character && isSelected) StartCoroutine(DeactivateVfxRoutine());
            });
            Event.Register<Note[]>(OutcomePhase.EventType.OnNoteCompleted, notes =>
            {
                if (isSelected) StartCoroutine(DeactivateVfxRoutine());
            });

            if (musicVfx == null) return;
            
            instrumentBone = skeleton.Skeleton.FindBone("harp");
            var pos = new Vector2(instrumentBone.WorldX, instrumentBone.WorldY);
            musicVfxOffset = (Vector2)musicVfx.transform.position - ((Vector2)transform.position + pos);
        }
        private IEnumerator DeactivateVfxRoutine()
        {
            selectionVfx.SetTrigger("Out");
            yield return new WaitForSeconds(0.2f);

            isSelected = false;
            selectionVfx.gameObject.SetActive(false);
        }
        
        void Update()
        {
            if (!isPlaying) return;
            
            var pos = new Vector2(instrumentBone.WorldX, instrumentBone.WorldY);
            musicVfx.transform.position = (Vector2)transform.position + pos + musicVfxOffset;
        }

        public void PlayMusic(Note note)
        {
            isPlaying = true;
            
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
                pooledMusic.Value.volume = volume;
                pooledMusic.Value.Play();
            }
            
            musicVfx.SetTrigger("In");
            animator.SetBool("IsPlaying", true);
        }
        void OnMusicEnd()
        {
            isPlaying = false;
            
            pooledMusic.OnDone -= OnMusicEnd;
            
            musicVfx.SetTrigger("Out");
            animator.SetBool("IsPlaying", false);
        }

        public void BeginTalking() => animator.SetBool("IsTalking", true);
        public void StopTalking() => animator.SetBool("IsTalking", false);
    }
}