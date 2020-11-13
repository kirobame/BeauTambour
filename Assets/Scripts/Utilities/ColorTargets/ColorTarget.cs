using UnityEngine;

namespace BeauTambour
{
    public abstract class ColorTarget : MonoBehaviour
    {
        public abstract Color StartingColor { get; }
        public abstract float StartingAlpha { get; }
        
        public virtual void Initialize() { }
        
        public abstract void Set(Color color);
        public abstract void SetAlpha(float alpha);
    }
}