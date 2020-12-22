using Flux;
using UnityEngine;

namespace Deprecated
{
    [IconIndicator(2925954276054455316)]
    public class InputSequenceHandler : MonoBehaviour
    {
        [SerializeField] private InputSequence[] sequences;
        void Awake() { foreach(var sequence in sequences) sequence.Initialize(this); }
    }
}