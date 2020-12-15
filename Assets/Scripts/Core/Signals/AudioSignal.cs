using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "newAudioEffectSignal", menuName = "Beau Tambour/Signals/Audio")]
    public class AudioSignal : Signal
    {
        public override string Category => "sfx";

        [SerializeField] private CharacterAudioSignalRegistry characterAudioSignalRegistry;
        private PoolableAudio poolableAudio;
        
        public override void Execute(MonoBehaviour hook, Character character, string[] args)
        {
            var audioClips = characterAudioSignalRegistry[character];
            var audioClip = audioClips[Random.Range(0, audioClips.Values.Count)];

            var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
            poolableAudio = audioPool.RequestSinglePoolable();

            poolableAudio.OnDone += OnPoolableDeactivation;
            
            poolableAudio.Value.clip = audioClip;
            poolableAudio.Value.Play();
        }
        
        void OnPoolableDeactivation()
        {
            poolableAudio.OnDone -= OnPoolableDeactivation;
            End();
        }
    }
}