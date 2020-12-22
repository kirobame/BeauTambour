using System;

namespace Deprecated
{
    [Flags]
    public enum OutcomeType
    {
        None = 0,
        
        Default = 1,
        
        Emotion = 2,
        Conversation = 4,
        Dialogue = 8,
        Block = 16
    }
}