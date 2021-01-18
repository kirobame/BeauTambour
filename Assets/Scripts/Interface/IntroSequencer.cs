using System;
using System.Collections;
using Flux;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = Flux.Event;

namespace BeauTambour
{
    public class IntroSequencer : MonoBehaviour
    {
        [SerializeField] private InputMapReference mapReference;

        [Space, SerializeField] private string key;
        [SerializeField] private int sheetIndex;
        [SerializeField] private int range;

        [Space, SerializeField] private VerticalLayoutGroup layout;
        [SerializeField] private Animator nextBitIndicator;
        [SerializeField] private float spacing;
        
        private StoryBit[] storyBits;

        private int index;
        private bool isShowingText;
        
        void Awake()
        {
            Event.Open(ExtraEvents.OnIntroStoryEnd);
            Event.Open(ExtraEvents.OnIntroAllTextShown);
            
            Event.Open(ExtraEvents.OnIntroBitSkipped);
            Event.Register(ExtraEvents.OnIntroBitSkipped, OnIntroBitSkipped);
        }
        
        void Start()
        {
            mapReference.Value.Disable();
            StartCoroutine(BootupRoutine());
        }
        private IEnumerator BootupRoutine()
        {
            storyBits = new StoryBit[range];
            for (var i = 0; i < range; i++)
            {
                var storyBitPool = Repository.GetSingle<StoryBitPool>(References.StoryBitPool);
                storyBits[i] = storyBitPool.RequestSingle();
                
                storyBits[i].Execute(sheetIndex, $"{key}.{i+1}");
                storyBits[i].transform.parent = layout.transform;
            }

            layout.enabled = false;
            yield return new WaitForEndOfFrame();
            layout.enabled = true;
        }

        public void Begin()
        {
            mapReference.Value.Enable();
            SetupCurrentBit();
        }
        private void SetupCurrentBit()
        {
            isShowingText = true;
            
            storyBits[index].Show();
            storyBits[index].Player.onTextShowed.AddListener(OnTextShowed);
        }

        void OnTextShowed()
        {
            storyBits[index].Player.onTextShowed.RemoveListener(OnTextShowed);
            isShowingText = false;
            
            if (index + 1 >= range)
            {
                Event.Call(ExtraEvents.OnIntroAllTextShown);
                return;
            }

            var searchIndex = 1;
            var info = storyBits[index].TextMesh.textInfo;
            var charInfo = info.characterInfo[info.characterCount - searchIndex];

            while (!char.IsLetter(charInfo.character))
            {
                searchIndex++;
                charInfo = info.characterInfo[info.characterCount - searchIndex];
            }
            var point = charInfo.bottomRight + (charInfo.topRight - charInfo.bottomRight) * 0.5f;
                
            nextBitIndicator.transform.position = storyBits[index].transform.TransformPoint(point) + Vector3.right * (searchIndex * spacing);
            nextBitIndicator.SetTrigger("In");
        }

        void OnIntroBitSkipped()
        {
            if (isShowingText) storyBits[index].Player.SkipTypewriter();
            else
            {
                if (index + 1 >= range)
                {
                    mapReference.Value.Disable();
                    Event.Call(ExtraEvents.OnIntroStoryEnd);
                    
                    return;
                }

                nextBitIndicator.SetTrigger("Out");
                
                index++;
                SetupCurrentBit();
            }
        }
    }
}