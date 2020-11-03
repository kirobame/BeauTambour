using Flux;

namespace BeauTambour
{
    [EnumAddress, TrackEnumReferencing]
    public enum Reference
    {
        Settings,
        RythmHandler,
        LinePool,
        StickIndicator
    }
}