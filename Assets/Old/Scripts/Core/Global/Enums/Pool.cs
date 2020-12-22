using Flux;

namespace Deprecated
{
    [EnumAddress, TrackEnumReferencing]
    public enum Pool
    {
        Drawing, 
        Audio,
        VisualEffect
    }
}