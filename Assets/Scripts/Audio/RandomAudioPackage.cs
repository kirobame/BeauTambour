using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewRandomAudioPackage", menuName = "Beau Tambour/General/Packages/Random Audio")]
    public class RandomAudioPackage : AudioPackage
    {
        [SerializeField] private AudioPackage[] packages;
        
        public override void AssignTo(AudioSource source, EventArgs inArgs)
        {
            var index = Random.Range(0, packages.Length);
            packages[index].AssignTo(source, inArgs);
        }
    }
}