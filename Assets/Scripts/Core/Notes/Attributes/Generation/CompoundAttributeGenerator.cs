using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-1933036960013795040), CreateAssetMenu(fileName = "NewAttributeGenerator", menuName = "Beau Tambour/Inputs/Attribute Generator")]
    public class CompoundAttributeGenerator : ScriptableObject
    {
        [SerializeField] private NoteAttributeGenerator[] subGenerators;
        
        public IEnumerable<NoteAttribute> Generate()
        {
            var attributes = new List<NoteAttribute>();
            foreach (var subGenerator in subGenerators)
            {
                if (!subGenerator.TryGenerate(out var noteAttribute)) continue;
                attributes.Add(noteAttribute);
            }

            return attributes;
        }
    }
}