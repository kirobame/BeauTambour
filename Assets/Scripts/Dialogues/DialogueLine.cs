using UnityEngine;

namespace BeauTambour
{
    public class DialogueLine : MonoBehaviour
    {
        public RectTransform RectTransform => rectTransform;
        
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Place(Vector2 position, bool onRight)
        {
            rectTransform.position = position;
            
            if (onRight)
            {
                spriteRenderer.flipX = false;
                rectTransform.pivot = new Vector2(1,0);
            }
            else
            {
                spriteRenderer.flipX = true;
                rectTransform.pivot = new Vector2(0,0);
            }
        }
    }
}