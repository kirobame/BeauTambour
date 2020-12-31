using Flux;

namespace BeauTambour
{
    [TrackEnumReferencing, EnumAddress]
    public enum References
    {
        Camera,
    
        Characters,
        Encounter,
        
        AudioPool,
        AnimationPool,
        DrawingPool,
        
        DrawingsParent,
        
        PhaseHandler,
        DialogueHandler,
        DrawingHandler,
        
        ColorByEmotion,
        
        PauseMenu,
        
        Settings,
    }
}