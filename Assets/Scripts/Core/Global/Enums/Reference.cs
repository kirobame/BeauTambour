﻿using Flux;

namespace BeauTambour
{
    [EnumAddress, TrackEnumReferencing]
    public enum Reference
    {
        Settings,
        RythmHandler,
        StickIndicator,
        DialogueProvider,
        RuntimeSettings,
        PhaseHandler,
        DialogueManager,
        Camera
    }
}