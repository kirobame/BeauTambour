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
        [SerializeField] private float errorOffset = 0.2f;// 45 degrees max per emotion (max 0.5 as dotResult)
        [SerializeField] private Emotion emotion;
        [SerializeField] private float catchZonedistance = 0.3f;

        private float maxScale = 1.0f;
        private float minScale = 0.5f;
        
        private float velocity;

        public Emotion Emotion
        {
            get
            {
                return emotion;
            }
        }

        public Transform Anchor { get => anchor;}

        public bool IsInSection(Vector2 input)
        {
            float dotResult = Vector2.Dot(input.normalized, anchor.localPosition);
            if (dotResult >= 1 - errorOffset)
            {
                Debug.Log(emotion);
                return true;
            }
            return false;
        }

        public float GetDistanceWithAnchor(Vector2 input)
        {
            Vector2 diff = (Vector2)anchor.localPosition - input;
            return diff.magnitude;
        }

        public void ScaleAnchor(Vector2 input)
        {
            anchor.localScale = new Vector2(1, 1) * Mathf.Lerp(maxScale,minScale,GetDistanceWithAnchor(input));
        }

        public void SmoothScaleAnchorReset()
        {
            anchor.localScale = new Vector2(1, 1) * Mathf.SmoothDamp(anchor.localScale.x, minScale, ref velocity, 0.5f);
        }

        public bool IsInCatchZone(Vector2 input)
        {
            return GetDistanceWithAnchor(input) <= catchZonedistance?true:false;
        }
    }
}