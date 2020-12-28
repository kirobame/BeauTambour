using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewAudioCollectionPackage", menuName = "Beau Tambour/General/Packages/Audio Collection")]
    public class AudioCollectionPackage : AudioPackage
    {
        [SerializeField] private AudioPackage[] packages;
        
        public override void AssignTo(AudioSource source, EventArgs inArgs)
        {
            if (!(inArgs is IndexArgs indexArgs) || indexArgs.Value >= packages.Length) return;
            packages[indexArgs.Value].AssignTo(source, inArgs);
        }
    }
}