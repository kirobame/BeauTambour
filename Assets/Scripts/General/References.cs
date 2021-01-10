using Flux;

namespace BeauTambour
{
    [TrackEnumReferencing, EnumAddress]
    public enum References
    {
        Camera,
    
        Characters,
        Encounter,
        
        ImagePool,
        IconPool,
        AudioPool,
        AnimationPool,
        DrawingPool,
        
        DrawingsParent,
        
        PhaseHandler,
        DialogueHandler,
        DrawingHandler,
        
        TextProvider,
        ColorByEmotion,
        
        PauseMenu,
        
        Settings,
    }
}