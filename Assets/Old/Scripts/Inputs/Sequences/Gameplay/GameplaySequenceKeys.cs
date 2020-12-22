using System;

namespace Deprecated
{
    [Flags]
    public enum GameplaySequenceKeys
    {
        Start,
        
        PickMusician,
        ClearNotes,
        CompletePartition, 
        
        End
    }
}