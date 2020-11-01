using System;
using UnityEngine;

namespace BeauTambour
{
    [Serializable]
    public struct Point
    {
        public Point(Vector2 position, float toleranceRadius)
        {
            this.position = position;
            this.toleranceRadius = toleranceRadius;
        }
    
        public Vector2 position;
        public float toleranceRadius;
    }
}