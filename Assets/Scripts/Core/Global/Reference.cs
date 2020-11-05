using Flux;

namespace BeauTambour
{
    [EnumAddress, TrackEnumReferencing]
    public enum Reference
    {
        Settings,
        RythmHandler,
        DrawingPool,
        StickIndicator,
        DialogueProvider
    }
}