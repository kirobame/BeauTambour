using System.Collections;
using UnityEngine;

namespace BeauTambour
{
    public class DialogueBounds : MonoBehaviour
    {
        public Color color
        {
            get => renderer.color;
            set => renderer.color = value;
        }
        public float Width => rectTransform.rect.width;
        
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform texTransform;
        [SerializeField] private new SpriteRenderer renderer;
        
        [Space, SerializeField] private LineRenderer line;
        [SerializeField] private float offset;

        private float initialWidth;

        void Awake() => initialWidth = rectTransform.sizeDelta.x;

        public void Reboot() // Prepare bounds to correctly calculate width a height
        {
            rectTransform.sizeDelta = new Vector2(initialWidth, 1000f);
            texTransform.sizeDelta = new Vector2(initialWidth, 1000f);
        }

        public void Place(Vector2 position, Vector2 lineAnchor, Vector2 direction)
        {
            rectTransform.position = position;

            var spriteBounds = renderer.bounds;
            spriteBounds.Expand(-0.5f);
            
            var end = (Vector2)spriteBounds.ClosestPoint(lineAnchor);
            end += direction * offset;
            
            line.SetPosition(0, lineAnchor);
            line.SetPosition(1, end);
        }
        public void Resize(Vector2 size)
        {
            rectTransform.sizeDelta = size;
            texTransform.sizeDelta = size;
            
            renderer.size = size;
        }
    }
}