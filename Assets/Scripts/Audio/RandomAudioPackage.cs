using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewRandomAudioPackage", menuName = "Beau Tambour/General/Packages/Random Audio")]
    public class RandomAudioPackage : AudioPackage
    {
        [SerializeField] private AudioPackage[] packages;

        private int leftOutIndex = -1;
        private List<int> availableIndices = new List<int>();

        private bool hasBeenBootedUp = false;
        
        public override void AssignTo(AudioSource source, EventArgs inArgs)
        {
            if (!hasBeenBootedUp)
            {
                for (var i = 0; i < packages.Length; i++) availableIndices.Add(i);
                hasBeenBootedUp = true;
            }
            
            var index = availableIndices[Random.Range(0, availableIndices.Count)];
            availableIndices.Remove(index);
            packages[index].AssignTo(source, inArgs);
            
            if (leftOutIndex != -1)
            {
                availableIndices.Add(leftOutIndex);
                leftOutIndex = -1;
            }

            if (availableIndices.Count == 0)
            {
                leftOutIndex = index;
                for (var i = 0; i < packages.Length; i++)
                {
                    if (i == index) continue;
                    availableIndices.Add(i);
                }
            }
        }
    }
}