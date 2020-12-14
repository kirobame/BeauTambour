using System;
using UnityEngine;

namespace BeauTambour
{
    [Serializable]
    public struct Block
    {
        public CompoundEmotion Emotion => emotion;
        public string Id => id;
        
        [SerializeField] private CompoundEmotion emotion;
        [SerializeField] private string id;
    }
}