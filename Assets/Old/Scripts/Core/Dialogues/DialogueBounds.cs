using Flux;
using System.Collections;
using System.Text.RegularExpressions;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Deprecated
{
    public class DialogueBounds : MonoBehaviour
    {
        public Color color
        {
            get => renderer.color;
            set => renderer.color = value;
        }
        public float Width => rectTransform.rect.width;
        public TextMeshPro TextMesh => textMesh;

        [Space, SerializeField] private TextMeshPro textMesh;
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
        
        [Space, SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform texTransform;
        [SerializeField] private new SpriteRenderer renderer;
        
        [Space, SerializeField] private LineRenderer line;
        [SerializeField] private float offset;

        private float initialWidth;
        private float sceneWidth = 31.95f;

        void Awake() => initialWidth = rectTransform.sizeDelta.x;

        public void Reboot() // Prepare bounds to correctly calculate width a height
        {
            rectTransform.sizeDelta = new Vector2(1000f, 1000f);
            texTransform.sizeDelta = new Vector2(1000f, 1000f);
        }

        public void Place(Vector2 position, Vector2 lineAnchor, Vector2 side, bool isLineVertical = false)
        {          
            rectTransform.position = position;
            ReplaceInCameraBounds(position, side);

            var spriteBounds = renderer.bounds;
            spriteBounds.Expand(-0.5f);
            
            var end = (Vector2)spriteBounds.ClosestPoint(lineAnchor);
            if(!isLineVertical)end += -side * offset;
            
            line.SetPosition(0, lineAnchor);
            line.SetPosition(1, end);
        }
        public void Resize(Vector2 size)
        {
            rectTransform.sizeDelta = size;
            texTransform.sizeDelta = size;
            
            renderer.size = size;
        }

        private void ReplaceInCameraBounds(Vector2 position, Vector2 side)
        {
            Camera cam = Repository.GetSingle<Camera>(Reference.Camera);
            var camWidth = cam.orthographicSize * 2f * cam.aspect;
            var diff = 0f;
            if (side.x == -1)
            {
                diff = position.x - (cam.rect.position.x + (sceneWidth / 2f) * side.x);
                if(diff < 0)
                {
                    rectTransform.position = rectTransform.position + new Vector3(diff - 0.5f, 0) * side.x;
                }
            }
            else if (side.x == 1)
            {
                diff = (position.x + rectTransform.rect.width) - (cam.rect.position.x + (sceneWidth / 2f) * side.x);
                if (diff > 0)
                {
                    rectTransform.position = rectTransform.position - new Vector3(diff + 0.5f, 0) * side.x;
                }
            }
        }

        public void SetText(string text)
        {
            textAnimatorPlayer.StopShowingText();
            textAnimatorPlayer.ShowText(text);
            textAnimatorPlayer.StartShowingText();
        }
    }
}