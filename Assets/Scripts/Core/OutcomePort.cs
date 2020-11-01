using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    public class OutcomePort : ScriptableObject
    {
        public bool IsValid => advancement.All(item => item.type == NoteAttributeType.Null);
        public IEnumerable<NoteAttribute> Attributes => advancement.Select(item => item.attribute);
        
        public IReadOnlyList<NoteAttributeType> Keys => keys;
        [SerializeField] private NoteAttributeType[] keys;

        private (NoteAttribute attribute, NoteAttributeType type)[] advancement;

        void OnEnable()
        {
            advancement = new (NoteAttribute attribute, NoteAttributeType type)[keys.Length];
            Reboot();
        }
        
        public void Reboot()
        {
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                advancement[i] = (null, key);
            }
        }
        public void TryAdvance(NoteAttribute attribute)
        {
            for (var i = 0; i < advancement.Length; i++)
            {
                if (advancement[i].type != attribute.Type) continue;

                advancement[i] = (attribute, NoteAttributeType.Null);
                break;
            }
        }
    }
}