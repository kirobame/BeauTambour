using Flux.Editor;
using UnityEditor;

namespace Deprecated.Editor
{
    [CustomCollection(typeof(NoteAttributeGenerator))]
    public class NoteAttributeGeneratorCollectionDrawer : EmbeddedScriptableCollectionDrawer<NoteAttributeGenerator>
    {
        public NoteAttributeGeneratorCollectionDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property) { }
    }
}