using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Frog : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [Space, SerializeField] private AudioPackage hitAudio;

        private bool isRightHit;

        void Awake()
        {
            Event.Open<string>(GameEvents.OnFrogFeedback);
            Event.Register<string>(GameEvents.OnFrogFeedback, OnFrogFeedback);
        }

        public void OnHit()
        {
            var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
            var audioSource = audioPool.RequestSingle();
            
            hitAudio.AssignTo(audioSource, EventArgs.Empty);
            audioSource.Play();
        }
        
        void OnFrogFeedback(string code)
        {
            switch (code)
            {
                case "Hit" :

                    if (isRightHit)
                    {
                        animator.SetTrigger("PlayingDrum1");
                        isRightHit = false;
                    }
                    else
                    {
                        animator.SetTrigger("PlayingDrum2");
                        isRightHit = true;
                    }
                    break;
            }
        }
    }
}