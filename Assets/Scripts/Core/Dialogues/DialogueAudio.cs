using BeauTambour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class DialogueAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioFillTempTab;

        private Dictionary<char, AudioClip> audioDict;
        [SerializeField] private AudioSource source;
        private bool isStarted = false;
        private AudioClip[] cueAudio;
        private int currentIndexInAudio = 0;
        [SerializeField] private float audioDelay = 0.01f;
        private float timeSinceLastAudio = 0;

        // Start is called before the first frame update
        private void Start()
        {
            Event.Register(DialogueManager.EventType.OnEnd, Stop);
            Event.Register<int, string>(DialogueManager.EventType.OnNext, Begin);

            audioDict = new Dictionary<char, AudioClip>();
            int start = 'A';
            for (int index = 0;index < audioFillTempTab.Length-1;index++)
            {
                audioDict.Add((char)(start + index), audioFillTempTab[index]);
            }
            audioDict.Add(' ',audioFillTempTab[audioFillTempTab.Length-1]);
        }

        private void Update()
        {
            if (!isStarted) return;

            if (Time.time - timeSinceLastAudio >= audioDelay)
            {
                timeSinceLastAudio = Time.time;

                source.clip = cueAudio[currentIndexInAudio];
                source.Play();
                currentIndexInAudio++;
                if (currentIndexInAudio >= cueAudio.Length)
                {
                    Stop();
                    currentIndexInAudio = 0;
                }
            }
           
            /*if (Time.time - timeSinceStart <= length)
            {
                int randIndex = Random.Range(0, audioFillTempTab.Length);
                source.clip = audioFillTempTab[randIndex];
                source.Play();
            }
            else
            {
                Stop();
            }*/
        }

        public void Begin(int advancement, string text)
        {
            isStarted = true;
            cueAudio = new AudioClip[text.Length];
            for (int index = 0; index < text.Length; index++)
            {
                int asciiCodeCar = (int)char.ToUpper(text[index]);
                AudioClip clip;
                if(audioDict.TryGetValue((char)asciiCodeCar,out clip))
                {
                    cueAudio[index] = clip;
                }
                else
                {
                    cueAudio[index] = audioDict[' '];
                }
            }
            timeSinceLastAudio = Time.time - audioDelay;
        }

        public void Stop()
        {
            isStarted = false;
        }
    }
}

