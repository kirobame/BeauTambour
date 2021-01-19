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
        StoryBitPool,
        
        DrawingsParent,
        
        PhaseHandler,
        DialogueHandler,
        DrawingHandler,
        MusicHandler,
        
        TextProvider,
        ColorByEmotion,
        
        BlockInfo,
        PauseMenu,

        OperationHandler,
        
        Settings,
    }
}