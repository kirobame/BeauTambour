using Flux;

namespace BeauTambour
{
    [EnumAddress]
    public enum GameEvents
    {
        OnEncounterBootedUp,
        
        OnNextCue,
        OnDialogueFinished,
        
        OnBlockPassed,
    }
}