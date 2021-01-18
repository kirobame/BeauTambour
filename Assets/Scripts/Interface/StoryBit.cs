using TMPro;
using UnityEngine;

namespace BeauTambour
{
    public class StoryBit : MonoBehaviour
    {
        public TMP_Text Text => text;
        [SerializeField] private TMP_Text text;
        
        public float WaitTime => waitTime;
        [SerializeField] private float waitTime;
    }
}