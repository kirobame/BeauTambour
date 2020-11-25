using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(2925954276054455316)]
    public class InputSequenceHandler : MonoBehaviour
    {
        [SerializeField] private InputSequence[] sequences;
        
        void Awake() { foreach(var sequence in sequences) sequence.Initialize(this); }
        
        void Update() { foreach(var continuousHandler in sequences.SelectMany(sequence => sequence.ContinuousHandlers)) continuousHandler.Update(); }
    }
}