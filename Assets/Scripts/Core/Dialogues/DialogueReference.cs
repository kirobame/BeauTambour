using System;
using Flux;
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

        public Dialogue Value
        {
            get
            {
                var provider = Repository.GetSingle<DialogueProvider>(Reference.DialogueProvider);
                return provider[this];
            }
        }

        public int EncounterIndex => encounterIndex;
        public string Id => id;
        
        [SerializeField] private int encounterIndex;
        [SerializeField] private string id;
    }
}