using Flux.Editor;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CustomEditor(typeof(Outcome))]
    public class OutcomeEditor : ScriptableObjectEditor
    {
        private EffectSequenceDrawer effectsDrawer;

        protected override void Initialize()
        {
            base.Initialize();

            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            
            for (var i = 0; i < 5; i++) iterator.NextVisible(false);
            var copy = iterator.Copy();
            
            for (var i = 0; i < 2; i++) iterator.NextVisible(false);
            copy.objectReferenceValue = iterator.objectReferenceValue;

            serializedObject.ApplyModifiedProperties();
        }

        protected override void Draw(SerializedProperty iterator)
        {
            iterator.NextVisible(false);
            EditorGUILayout.PropertyField(iterator);
            
            iterator.NextVisible(false);
            EditorGUILayout.PropertyField(iterator);
            
            iterator.NextVisible(false);
            EditorGUILayout.PropertyField(iterator);
            
            iterator.NextVisible(false);
            EditorGUILayout.PropertyField(iterator);
            
            iterator.NextVisible(false);
            EditorGUILayout.PropertyField(iterator);
            
            iterator.NextVisible(false);
            DrawCollection(iterator);
            
            iterator.NextVisible(false);
            if (iterator.objectReferenceValue == null) return;

            if (effectsDrawer == null)
            {
                var serializedObject = new SerializedObject(iterator.objectReferenceValue);
                var effectsProperty = serializedObject.FindProperty("effects");
                
                effectsDrawer = new EffectSequenceDrawer(serializedObject, effectsProperty);
            }

            effectsDrawer.Draw();
            effectsDrawer.SerializedObject.ApplyModifiedProperties();
        }
    }
}