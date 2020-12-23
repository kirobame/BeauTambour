using System;
using System.Collections;
using Febucci.UI;
using Flux;
using TMPro;
using UnityEngine;

namespace BeauTambour
{
    public class DialogueHolder : MonoBehaviour
    {
        public Color color
        {
            get => boundsRenderer.color;
            set => boundsRenderer.color = value;
        }
        public TextMeshPro TextMesh =>  textMesh;
        
        [SerializeField] private SpriteRenderer boundsRenderer;
        [SerializeField] private RectTransform boundsTransform;
        [SerializeField] private RectTransform textTransform;
        
        [Space, SerializeField] private DialogueLine dialogueLine;
        [SerializeField] private AnimationCurve renewCurve;
        [SerializeField] private AnimationCurve refreshCurve;
        
        private TextMeshPro textMesh;
        private TextAnimatorPlayer textAnimatorPlayer;

        private Vector2 previousSize;

        void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
            textAnimatorPlayer = GetComponent<TextAnimatorPlayer>();
        }

        public void Bootup()
        {
            previousSize = boundsTransform.sizeDelta;
            
            boundsTransform.sizeDelta = new Vector2(1000f, 1000f);
            textTransform.sizeDelta = new Vector2(1000f, 1000f);
        }
        
        public void Renew(Vector2 position, Vector2 size, string text) => StartCoroutine(RenewRoutine(position, size, text));
        private IEnumerator RenewRoutine(Vector2 position, Vector2 size, string text)
        {
            ToggleTextAnimation(false);
            
            var time = 0f;
            var goal = 0.4f;
            
            while (time < goal)
            {
                var ratio = time / goal;
                dialogueLine.RectTransform.localScale = Vector3.one * renewCurve.Evaluate(1.0f - ratio);

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            textMesh.text = string.Empty;
            
            var camera = Repository.GetSingle<Camera>(References.Camera);
            var screenPosition = camera.WorldToViewportPoint(position);
            
            boundsTransform.sizeDelta = size;
            boundsRenderer.size = size;
            
            textTransform.sizeDelta = size;

            if (screenPosition.x > 0.5f)
            {
                dialogueLine.Place(position, true);

                if (size.x > dialogueLine.RectTransform.sizeDelta.x * 2.0f) SetBoundsX(-size.x);
                else SetBoundsX((-size.x / 2.0f) - dialogueLine.RectTransform.sizeDelta.x);
            }
            else
            {
                dialogueLine.Place(position, false);

                if (size.x > dialogueLine.RectTransform.sizeDelta.x * 2.0f) SetBoundsX(0.0f);
                else SetBoundsX(dialogueLine.RectTransform.sizeDelta.x - (size.x / 2.0f));
            }

            time = 0f;
            while (time < goal)
            {
                var ratio = time / goal;
                dialogueLine.RectTransform.localScale = Vector3.one * renewCurve.Evaluate(ratio);
                
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            ToggleTextAnimation(true);
            SetText(text);
        }

        public void Refresh(Vector2 size, string text) => StartCoroutine(RefreshRoutine(size, text));
        private IEnumerator RefreshRoutine(Vector2 size, string text)
        {
            ToggleTextAnimation(false);
            
            boundsTransform.sizeDelta = previousSize;
            boundsRenderer.size = previousSize;
                
            textTransform.sizeDelta = previousSize;
            
            var time = 0f;
            var goal = 0.25f;
            
            while (time < goal)
            {
                var ratio = time / goal;
                textMesh.alpha = refreshCurve.Evaluate(1.0f - ratio);

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            time = 0f;
            while (time < goal)
            {
                var ratio = time / goal;
                var lerpedSize = Vector2.Lerp(previousSize, size, refreshCurve.Evaluate(ratio));
                
                boundsTransform.sizeDelta = lerpedSize;
                boundsRenderer.size = lerpedSize;
                
                textTransform.sizeDelta = lerpedSize;
                
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            textMesh.alpha = 1f;
            ToggleTextAnimation(true);
            
            SetText(text);
        }

        private void ToggleTextAnimation(bool state)
        {
            if (state == true)
            {
                textAnimatorPlayer.textAnimator.enabled = true;
                textAnimatorPlayer.enabled = true;
            }
            else
            {
                textAnimatorPlayer.StopShowingText();
                
                textAnimatorPlayer.textAnimator.enabled = false;
                textAnimatorPlayer.enabled = false;
            }
        }
        private void SetBoundsX(float xValue)
        {
            var boundsPosition = boundsTransform.localPosition;
            boundsPosition.x = xValue;
            boundsTransform.localPosition = boundsPosition;
        }
        private void SetText(string text)
        {
            textAnimatorPlayer.StopShowingText();
            textAnimatorPlayer.ShowText(text);
            textAnimatorPlayer.StartShowingText();
        }
    }
}