using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-1684237209172799180), CreateAssetMenu(fileName = "NewCompoundEmotion", menuName = "Beau Tambour/Compound Emotion")]
    public class CompoundEmotion : ScriptableObject
    {
        [SerializeField] private Emotion[] emotions;
        
        public List<Emotion> GetCopy() => new List<Emotion>(emotions);
    }
}