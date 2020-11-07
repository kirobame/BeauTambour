using System;
using UnityEngine;

namespace BeauTambour
{
    [Serializable]
    public struct DialogueReference
    {
        public DialogueReference(int encounterIndex, string id)
        {
            this.encounterIndex = encounterIndex;
            this.id = id;
        }

        public int EncounterIndex => encounterIndex;
        public string Id => id;
        
        [SerializeField] private int encounterIndex;
        [SerializeField] private string id;
    }
}