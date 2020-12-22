using Flux.Editor;
using UnityEditor;

namespace Deprecated.Editor
{
    [CustomCollection(typeof(CharacterInput))]
    public class CharacterInputCollectionDrawer : EmbeddedScriptableCollectionDrawer<CharacterInput>
    {
        public CharacterInputCollectionDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property) { }
    }
}