using System;

namespace BeauTambour
{
    [Flags]
    public enum OutcomeType
    {
        None = 0,
        
        Default = 1,
        
        Emotion = 2,
        Conversation = 4
    }
}