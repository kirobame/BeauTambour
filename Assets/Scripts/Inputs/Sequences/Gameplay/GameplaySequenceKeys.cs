using System;

namespace BeauTambour
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