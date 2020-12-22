using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace BeauTambour
{
    public class DialogueHolder : MonoBehaviour
    {
        public Color color
        {
            get => renderer.color;
            set => renderer.color = value;
        }
        public TextMeshPro TextMesh => textMesh;
        
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
        
        [Space, SerializeField] private new SpriteRenderer renderer;
        
        [Space, SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform texTransform;
        
        public void Reboot()
        {
            rectTransform.sizeDelta = new Vector2(1000f, 1000f);
            texTransform.sizeDelta = new Vector2(1000f, 1000f);
        }
        
        public void Place(Vector2 position, Vector2 size)
        {
            rectTransform.position = position;
            
            rectTransform.sizeDelta = size;
            texTransform.sizeDelta = size;
            renderer.size = size;

            StartCoroutine(ScaleRoutine());
        }
        private IEnumerator ScaleRoutine()
        {
            yield break;
        }

        public void Resize(Vector2 size) => StartCoroutine(ResizeRoutine(size));
        private IEnumerator ResizeRoutine(Vector2 size)
        {
            rectTransform.sizeDelta = size;
            texTransform.sizeDelta = size;
            renderer.size = size;
            
            yield break;
        }
        
        public void SetText(string text)
        {
            textAnimatorPlayer.StopShowingText();
            textAnimatorPlayer.ShowText(text);
            textAnimatorPlayer.StartShowingText();
        }
    }
}