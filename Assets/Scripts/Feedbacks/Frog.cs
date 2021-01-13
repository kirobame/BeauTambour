using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Frog : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AudioPackage[] hitAudios;

        private int rightHitSoundIndex;
        private int leftHitSoundIndex;
        
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
            
            hitAudios[isRightHit? leftHitSoundIndex : rightHitSoundIndex].AssignTo(audioSource, EventArgs.Empty);
            audioSource.Play();
        }
        
        void OnFrogFeedback(string code)
        {
            var splittedCode = code.Split('.');
            
            var name = splittedCode[0];
            var subCode = int.Parse(splittedCode[1]);

            switch (name)
            {
                case "Hit" :

                    if (isRightHit)
                    {
                        animator.SetTrigger("PlayingDrum1");
                        rightHitSoundIndex = subCode;
                        
                        isRightHit = false;
                    }
                    else
                    {
                        animator.SetTrigger("PlayingDrum2");
                        leftHitSoundIndex = subCode;
                        
                        isRightHit = true;
                    }
                    break;
            }
        }
    }
}