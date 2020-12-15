using System;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CreateAssetMenu(fileName = "NewMusicianInterpreter", menuName = "Beau Tambour/Editor/Outcome Interpreters/Needs Musician")]
    public class NeedsMusicianInterpreter : ConditionInterpreter<MusicianCondition>
    {
        [SerializeField] private ActorCharacterRegistry actorCharacterRegistry;
        
        protected override void HandleCondition(string data, Outcome outcome, MusicianCondition condition)
        {
            var serializedObject = new SerializedObject(condition);
            var targetProperty = serializedObject.FindProperty("musician");

            targetProperty.objectReferenceValue = actorCharacterRegistry.GetSafe((Actor)Enum.Parse(typeof(Actor), data));
            serializedObject.ApplyModifiedProperties();
        }
    }
}