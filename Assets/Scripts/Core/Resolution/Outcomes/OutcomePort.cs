using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(160919518743690994), CreateAssetMenu(fileName = "NewOutcomePort", menuName = "Beau Tambour/Resolution/Outcome Port")]
    public class OutcomePort : ScriptableObject
    {
        public bool IsValid => advancement.All(item => item.type == NoteAttributeType.Null);
        public IEnumerable<NoteAttribute> Attributes => advancement.Select(item => item.attribute);
        
        public IReadOnlyList<NoteAttributeType> Keys => keys;
        [SerializeField] private NoteAttributeType[] keys;

        private (NoteAttribute attribute, NoteAttributeType type)[] advancement;

        public void BootUp()
        {
            advancement = new (NoteAttribute attribute, NoteAttributeType type)[keys.Length];
            Reboot();
        }
        //
        public void Reboot()
        {
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                advancement[i] = (null, key);
            }
        }
        public void TryAdvance(NoteAttribute attribute, NoteAttributeType key)
        {
            for (var i = 0; i < advancement.Length; i++)
            {
                if (advancement[i].type != key) continue;

                advancement[i] = (attribute, NoteAttributeType.Null);
                break;
            }
        }
    }
}