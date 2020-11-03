using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public class SpriteRendererColorProxy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetAlpha(float alpha)
        {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}