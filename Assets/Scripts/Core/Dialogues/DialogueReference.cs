using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [Serializable]
    public struct DialogueReference
    {
        public DialogueReference(string encounterId, string id)
        {
            this.encounterId = encounterId;
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

        public string EncounterId => encounterId;
        public string Id => id;
        
        [SerializeField] private string encounterId;
        [SerializeField] private string id;
    }
}