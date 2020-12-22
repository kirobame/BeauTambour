using System;
using UnityEngine;

namespace Deprecated
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