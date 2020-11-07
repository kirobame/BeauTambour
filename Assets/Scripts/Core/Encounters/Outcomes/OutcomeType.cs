using System;

namespace BeauTambour
{
    [Flags]
    public enum OutcomeType
    {
        None = 0,
        
        PrimaryEmotion = 1,
        SecondaryEmotion = 2,
        TertiaryEmotion = 4
    }
}