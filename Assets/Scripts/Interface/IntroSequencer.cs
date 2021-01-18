using System.Collections;
using Flux;
using TMPro;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class IntroSequencer : MonoBehaviour
    {
        [SerializeField] private InputMapReference mapReference;
        
        [Space, SerializeField] private StoryBit[] bits;
        [SerializeField] private AnimationCurve apparitionCurve;
        [SerializeField] private float apparitionTime;

        private int index;
        private Coroutine apparitionRoutine;
        
        void Awake()
        {
            index = 0;

            Event.Open(ExtraEvents.OnIntroStoryEnd);
            
            Event.Open(ExtraEvents.OnIntroBitSkipped);
            Event.Register(ExtraEvents.OnIntroBitSkipped, OnIntroBitSkipped);
        }

        public void Begin() => apparitionRoutine = StartCoroutine(ApparitionRoutine());
        private IEnumerator ApparitionRoutine()
        {
            var time = 0.0f;
            while (time < apparitionTime)
            {
                Execute(time / apparitionTime);
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            Execute(1.0f);
            
            yield return new WaitForSeconds(bits[index].WaitTime);

            apparitionRoutine = null;
            if (index + 1 >= bits.Length)
            {
                End();
                yield break;
            }

            index++;
            apparitionRoutine = StartCoroutine(ApparitionRoutine());
        }
        private void Execute(float ratio)
        {
            var color = bits[index].Text.color;
            color.a = Mathf.Lerp(0.0f, 1.0f, apparitionCurve.Evaluate(ratio));
            bits[index].Text.color = color;
        }
        
        void OnIntroBitSkipped()
        {
            if (apparitionRoutine == null) return;
            
            StopCoroutine(apparitionRoutine);
            Execute(1.0f);

            if (index + 1 >= bits.Length)
            {
                End();
                return;
            }

            index++;
            apparitionRoutine = StartCoroutine(ApparitionRoutine());
        }

        private void End()
        {
            mapReference.Value.Disable();
            Event.Call(ExtraEvents.OnIntroStoryEnd);
        }
    }
}