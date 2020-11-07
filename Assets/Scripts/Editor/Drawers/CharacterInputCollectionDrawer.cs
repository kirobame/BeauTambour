using Flux.Editor;
using UnityEditor;

namespace BeauTambour.Editor
{
    [CustomCollection(typeof(CharacterInput))]
    public class CharacterInputCollectionDrawer : EmbeddedScriptableCollectionDrawer<CharacterInput>
    {
        public CharacterInputCollectionDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property) { }
    }
}