using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.VersionControl;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CreateAssetMenu(fileName = "NewIsEmotionMetInterpreter", menuName = "Beau Tambour/Editor/Outcome Interpreters/Is Emotion Met")]
    public class IsEmotionMetInterpreter : ConditionInterpreter<EmotionCondition>
    {
        private static Dictionary<string, CompoundEmotion> emotions = new Dictionary<string, CompoundEmotion>();
        
        [InitializeOnLoadMethod]
        private static void GetEmotions()
        {
            var guids = AssetDatabase.FindAssets("t:CompoundEmotion", new string[] {"Assets/Objects/Narration/Emotions"});
            emotions.Clear();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var emotion = AssetDatabase.LoadAssetAtPath<CompoundEmotion>(path);
                
                emotions.Add(emotion.name, emotion);
            }
        }
        
        protected override void HandleCondition(string data, Outcome outcome, EmotionCondition condition)
        {
            var serializedObject = new SerializedObject(condition);
            var targetProperty = serializedObject.FindProperty("target");

            targetProperty.objectReferenceValue = emotions[data];
            serializedObject.ApplyModifiedProperties();
        }
    }
}