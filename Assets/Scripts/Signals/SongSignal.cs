using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSongSignal", menuName = "Beau Tambour/Signals/Sfx")]
    public class SongSignal : Signal
    {
        #region Encapsulated Types

        [Serializable]
        public class Melody
        {
            public Character Key => key;
            [SerializeField] private Character key;

            public AudioPackage Value => value;
            [SerializeField] private AudioPackage value;
        }
        #endregion
        
        public override string Category => "sfx";

        [SerializeField] private PoolableAnimation effect;
        
        [SerializeField] private Melody[] stock;
        private Dictionary<int, AudioPackage> registry;

        private MonoBehaviour hook;
        
        private Animator usedEffect;
        private PoolableAudio usedAudio;
        
        public override void Bootup()
        {
            registry = new Dictionary<int, AudioPackage>();
            foreach (var melodies in stock) registry.Add(melodies.Key.Id, melodies.Value);
        }

        public override void Execute(MonoBehaviour hook, Character speaker, string[] args)
        {
            this.hook = hook;

            var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
            usedAudio = audioPool.RequestSinglePoolable();

            usedAudio.OnDone += OnSongEnd;
            registry[speaker.Id].AssignTo(usedAudio.Value, EventArgs.Empty);
            usedAudio.Value.Play();

            var animationPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
            usedEffect = animationPool.RequestSingle(effect);

            usedEffect.transform.parent = speaker.RuntimeLink.HeadSocket.Value;
            usedEffect.transform.localPosition = Vector3.zero;
            usedEffect.transform.localScale = Vector3.one;
            usedEffect.SetTrigger("In");
        }

        void OnSongEnd() => hook.StartCoroutine(EndRoutine());
        private IEnumerator EndRoutine()
        {
            usedAudio.OnDone -= OnSongEnd;
            usedEffect.SetTrigger("Out");
            
            yield return new WaitForSeconds(0.4f);
            
            End();
        }
    }
}