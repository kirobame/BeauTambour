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
        MusicHandler,
        
        TextProvider,
        ColorByEmotion,
        
        PauseMenu,

        Settings,
    }
}