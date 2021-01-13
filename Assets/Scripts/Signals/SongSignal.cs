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

        private Character speaker;
        private Animator usedEffect;
        private PoolableAudio usedAudio;
        
        public override void Bootup()
        {
            registry = new Dictionary<int, AudioPackage>();
            foreach (var melodies in stock) registry.Add(melodies.Key.Id, melodies.Value);
        }

        public override void Execute(MonoBehaviour hook, Character speaker, string[] args)
        {
            if (speaker is Musician || !registry.TryGetValue(speaker.Id, out var audioPackage))
            {
                hook.StartCoroutine(FallbackRoutine());
                return;
            }
            
            this.hook = hook;
            this.speaker = speaker;
            
            hook.StartCoroutine(ActivationRoutine(audioPackage));
        }
        private IEnumerator ActivationRoutine(AudioPackage audioPackage)
        {
            yield return hook.StartCoroutine(FadeMusicRoutine(false, 0.4f));
            
            speaker.RuntimeLink.Animator.SetBool("IsSinging", true);
            
            var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
            usedAudio = audioPool.RequestSinglePoolable();

            usedAudio.OnDone += OnSongEnd;
            audioPackage.AssignTo(usedAudio.Value, EventArgs.Empty);
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
            speaker.RuntimeLink.Animator.SetBool("IsSinging", false);

            yield return hook.StartCoroutine(FadeMusicRoutine(true, 0.4f));
            
            End();
        }
        private IEnumerator FallbackRoutine()
        {
            yield return new WaitForSeconds(0.2f);
            End();
        }

        private IEnumerator FadeMusicRoutine(bool shouldFadeIn, float duration)
        {
            var musicHandler = Repository.GetSingle<MusicHandler>(References.MusicHandler);
            musicHandler.Prepare();

            var time = 0.0f;
            while (time < duration)
            {
                if (shouldFadeIn) musicHandler.In(time / duration);
                else musicHandler.Out(time / duration);
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            
            if (shouldFadeIn) musicHandler.In(time / duration);
            else musicHandler.Out(time / duration);
        }
    }
}