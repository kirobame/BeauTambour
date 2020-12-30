using Flux;

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

        OnNarrativeEvent,
        
        OnBlockPassed,
    }
}