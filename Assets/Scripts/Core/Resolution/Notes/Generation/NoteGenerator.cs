using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-1933036960013795040), CreateAssetMenu(fileName = "NewNoteGenerator", menuName = "Beau Tambour/Notes/Generator")]
    public class NoteGenerator : ScriptableObject
    {
        [SerializeField] private NoteAttributeGenerator[] subGenerators;
        //
        public Note Generate()
        {
            var attributes = new List<NoteAttribute>();
            foreach (var subGenerator in subGenerators)
            {
                if (!subGenerator.TryGenerate(out var noteAttribute)) continue;
                attributes.Add(noteAttribute);
            }
            
            return new Note(attributes.ToArray());
        }
    }
}