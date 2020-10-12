using System;

namespace BeauTambour.Prototyping
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        
        Move = 1,
        Play = 2, 
        Power = 4,
        
        Standard = Move | Play | Power
    }
}