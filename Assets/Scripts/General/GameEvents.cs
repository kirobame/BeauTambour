﻿using Flux;

namespace BeauTambour
{
    [EnumAddress]
    public enum GameEvents
    {
        OnEncounterBootedUp,
        
        OnNextCue,
        OnCueFinished,
        OnCueSkipped,
        OnDialogueFinished,
        
        OnSpeakerSelected,

        OnNarrativeEvent,
        
        OnBlockPassed,
    }
}