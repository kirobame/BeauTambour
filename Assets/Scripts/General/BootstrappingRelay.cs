using System;
using UnityEngine;

namespace BeauTambour
{
    public static class BootstrappingRelay
    {
        public static int StartingBlock { get; private set; } = -1;
        public static int UseBackup { get; private set; } = -1;
        public static int PlayIntro { get; private set; } = -1;
        public static int SkipStart { get; private set; } = -1;

        public static void RevertToDefault()
        {
            StartingBlock = -1;
            UseBackup = -1;
            PlayIntro = -1;
            SkipStart = -1;
        }
            
        public static void SetStartingBlock(int index) => StartingBlock = index;
        
        public static void SetUseBackup(bool value) => UseBackup = Convert.ToInt32(value);
        public static void SetPlayIntro(bool value) => PlayIntro = Convert.ToInt32(value);
        public static void SetSkipStart(bool value) => SkipStart = Convert.ToInt32(value);
    }
}