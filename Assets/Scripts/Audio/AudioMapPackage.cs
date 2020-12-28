using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public abstract class AudioMapPackage<TKey> : AudioPackage
    {
        public AudioPackage this[TKey key] => packages[key];
        
        protected abstract KeyValuePair<TKey, AudioPackage>[] keyValuePairs { get; }
        private Dictionary<TKey, AudioPackage> packages;

        private bool hasBeenBootedUp;

        void OnDisable() => hasBeenBootedUp = false;

        private void BootUp()
        {
            packages = new Dictionary<TKey, AudioPackage>();
            foreach (var keyValuePair in keyValuePairs)
            {
                if (packages.ContainsKey(keyValuePair.Key)) continue;
                
                packages.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public override void AssignTo(AudioSource source, EventArgs inArgs)
        {
            if (!(inArgs is KeyArgs<TKey> keyArgs)) return;
            
            if (!hasBeenBootedUp)
            {
                BootUp();
                hasBeenBootedUp = true;
            }
            
            packages[keyArgs.Value].AssignTo(source, inArgs);
        }
        
        public bool TryGet(TKey key, out AudioPackage value)
        {
            if (!hasBeenBootedUp)
            {
                BootUp();
                hasBeenBootedUp = true;
            }

            if (packages.TryGetValue(key, out value)) return true;
            else return false;
        }
    }
}