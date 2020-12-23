using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deprecated;

namespace BeauTambour
{
    [System.Serializable]
    public class Section
    {
        [SerializeField] private Transform anchor;
        [SerializeField] private Vector2 dir;
        [SerializeField] private float errorOffset;// 45 degrees max per emotion (max 0.5 as dotResult)
        [SerializeField] private Emotion emotion;

        private float maxScale = 1.0f;
        private float minScale = 0.5f;
        private float velocity;

        public bool IsInSection(Vector2 position)
        {
            float dotResult = Vector2.Dot(position.normalized, dir);
            if (dotResult >= errorOffset)
            {
                Debug.Log(emotion);
                return true;
            }
            return false;
        }

        public float GetDistanceWithAnchor(Vector2 position)
        {
            Vector2 diff = dir - position;
            return Mathf.Clamp01(diff.magnitude);
        }

        public void ScaleAnchor(Vector2 position)
        {
            anchor.localScale = new Vector2(1, 1) * Mathf.Lerp(maxScale,minScale,GetDistanceWithAnchor(position));
        }

        public void SmoothScaleAnchorReset()
        {
            anchor.localScale = new Vector2(1, 1) * Mathf.SmoothDamp(anchor.localScale.x, minScale, ref velocity, 0.1f);
        }
    }
}