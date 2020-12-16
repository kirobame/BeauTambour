using Flux;

namespace BeauTambour
{
    [EnumAddress]
    public enum TempEvent
    {
        OnMusicianPicked,
        OnAnyMusicianPicked,
        OnPartitionCompleted,
        OnMusicianPickedExtended,
    }
}