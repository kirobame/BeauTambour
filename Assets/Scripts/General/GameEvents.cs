using Flux;

namespace BeauTambour
{
    [EnumAddress]
    public enum GameEvents
    {
        OnEncounterBootedUp,
        OnIntroStart,
        OnIntroEnd,
        OnIntroConfirmed,

        OnNextCue,
        OnCueFinished,
        OnCueSkipped,
        OnDialogueFinished,
        
        OnEmotionLatched,
        OnEmotionUnlatched,
        OnEmotionPicked,
        OnEmotionDrawingStart,
        OnEmotionDrawingCancellation,
        OnEmotionCancellationDone,
        OnEmotionDrawingEnd,
        
        OnDrawingStart,
        OnDraw,
        OnDrawingCancelled,
        OnDrawingColorReception,
        OnDrawingEnd,
        
        OnSpeakerSelected,
        OnSpeakerChoice,
        OnSpeakerChoiceDone,
        
        OnNoteValidation,
        OnNoteValidationDone,
        OnNoteDeletion,
        OnNoteDeletionDone,
        
        OnFrogFeedback,
        
        OnPauseToggled,
        OnGamePaused,
        OnGameResumed,

        OnDialogueTreeUpdate,
        OnNarrativeEvent,
        OnLanguageChanged,
        
        OnSpeakerEntrance,
        OnMusicianArcCompleted,
        
        OnInterlocutorConvinced,
        OnBlockPassed,
        OnEncounterEnd,
        
        OnStickUsed,
    }
}