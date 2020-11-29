using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewAdvanceSequenceOperation", menuName = "Beau Tambour/Operations/Advance Sequence")]
    public class AdvanceGameplaySequenceOperation : AdvanceSequenceOperation<GameplaySequenceKeys, GameplaySequenceElement, GameplaySequence> { }
}