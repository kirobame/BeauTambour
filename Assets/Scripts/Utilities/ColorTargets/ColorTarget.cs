using UnityEngine;

namespace BeauTambour
{
    public abstract class ColorTarget : MonoBehaviour
    {
        public abstract void Set(Color color);
        public abstract void SetAlpha(float alpha);
    }
}