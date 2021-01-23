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

        [Space, SerializeField] private float margin;
        [SerializeField] private bool useCorrection;
        
        private TextMeshPro textMesh;
        private TextAnimatorPlayer textAnimatorPlayer;

        private bool isOperational;
        private Vector2 previousSize;

        private float height;
        private float startX;
        private float maxWidth;
        
        private bool isOnRight;

        void Awake()
        {
            isOperational = false;
            height = boundsTransform.localPosition.y;
            
            textMesh = GetComponent<TextMeshPro>();
            textAnimatorPlayer = GetComponent<TextAnimatorPlayer>();

            textMesh.text = string.Empty;
            dialogueLine.RectTransform.localScale = Vector3.zero;
        }
        void Start()
        {
            var camera = Repository.GetSingle<Camera>(References.Camera);
            
            var start = camera.ViewportToWorldPoint(Vector2.zero);
            var end = camera.ViewportToWorldPoint(Vector2.one);

            startX = start.x + margin;
            maxWidth = (end - start).x - margin * 2.0f;
        }

        public void Bootup()
        {
            previousSize = boundsTransform.sizeDelta;
            
            boundsTransform.sizeDelta = new Vector2(1000f, 1000f);
            textTransform.sizeDelta = new Vector2(1000f, 1000f);
        }
        public void Revert()
        {
            boundsTransform.sizeDelta = previousSize;
            textTransform.sizeDelta = previousSize;
        }
        public void Offset(float value)
        {
            var position = boundsTransform.localPosition;
            position.y = height + value;
            boundsTransform.localPosition = position;
        }

        public void Deactivate() => StartCoroutine(DeactivationRoutine(true));
        private IEnumerator DeactivationRoutine(bool shutdown)
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
            
            if (shutdown)
            {
                isOperational = false;
                dialogueLine.RectTransform.localScale = Vector3.zero;
            }
        }
        
        public void Renew(Vector2 position, Vector2 size, string text) => StartCoroutine(RenewRoutine(position, size, text));
        private IEnumerator RenewRoutine(Vector2 position, Vector2 size, string text)
        {
            if (isOperational) yield return DeactivationRoutine(false);
            else isOperational = true;
            
            var camera = Repository.GetSingle<Camera>(References.Camera);
            var screenPosition = camera.WorldToViewportPoint(position);
            
            boundsTransform.sizeDelta = size;
            boundsRenderer.size = size;
            
            textTransform.sizeDelta = size;

            if (screenPosition.x > 0.5f)
            {
                dialogueLine.Place(position, true);

                PlaceOnRight(position, size);
                isOnRight = true;
            }
            else
            {
                dialogueLine.Place(position, false);

                PlaceOnLeft(position, size);
                isOnRight = false;
            }

            var time = 0f;
            var goal = 0.4f;
            
            while (time < goal)
            {
                var ratio = time / goal;
                dialogueLine.RectTransform.localScale = Vector3.one * renewCurve.Evaluate(ratio);
                
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            dialogueLine.RectTransform.localScale = Vector3.one * renewCurve.Evaluate(1);
            
            ToggleTextAnimation(true);
            SetText(text);
        }

        private void PlaceOnRight(Vector2 position, Vector2 size)
        {
            if (size.x > dialogueLine.RectTransform.sizeDelta.x * 2.0f)
            {
                var endX = Mathf.Abs(startX + maxWidth);
                var difference = endX - position.x;
                var correction = Mathf.Clamp(difference + size.x - maxWidth, 0.0f, float.PositiveInfinity);
                
                SetBoundsX(-size.x + correction);
            }
            else SetBoundsX((-size.x / 2.0f) - dialogueLine.RectTransform.sizeDelta.x);
        }
        private void PlaceOnLeft(Vector2 position, Vector2 size)
        {
            if (size.x > dialogueLine.RectTransform.sizeDelta.x * 2.0f)
            {
                var difference = Mathf.Abs(position.x - startX);
                var correction = Mathf.Clamp(difference + size.x - maxWidth, 0.0f, float.PositiveInfinity);
                
                SetBoundsX(-correction);
            }
            else SetBoundsX(dialogueLine.RectTransform.sizeDelta.x - (size.x / 2.0f));
        }

        public void Refresh(Vector2 size, string text) => StartCoroutine(RefreshRoutine(size, text));
        private IEnumerator RefreshRoutine(Vector2 size, string text)
        {
            ToggleTextAnimation(false);
            
            boundsTransform.sizeDelta = previousSize;
            boundsRenderer.size = previousSize;
                
            textTransform.sizeDelta = previousSize;
            
            var time = 0f;
            var goal = 0.2f;
            
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
                Execute(ratio);
                
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Execute(1);

            void Execute(float val)
            {
                var lerpedSize = Vector2.Lerp(previousSize, size, refreshCurve.Evaluate(val));

                var position = dialogueLine.transform.position;
                boundsTransform.sizeDelta = lerpedSize;
                boundsRenderer.size = lerpedSize;

                if (isOnRight) PlaceOnRight(position, lerpedSize);
                else PlaceOnLeft(position, lerpedSize);
                
                textTransform.sizeDelta = lerpedSize;
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