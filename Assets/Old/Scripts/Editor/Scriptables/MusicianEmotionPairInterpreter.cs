using System;
using UnityEditor;
using UnityEngine;

namespace Deprecated.Editor
{
    //[CreateAssetMenu(fileName = "NewMusicianEmotionPairInterpreter", menuName = "Beau Tambour/Editor/Outcome Interpreters/Musician Emotion Pair")]
    public class MusicianEmotionPairInterpreter : ConditionInterpreter<MusicianEmotionPairCondition>
    {
        [SerializeField] private ActorCharacterRegistry actorCharacterRegistry;
        
        protected override void HandleCondition(string data, Outcome outcome, MusicianEmotionPairCondition condition)
        {
            var split = data.Split('/');
            
            var serializedObject = new SerializedObject(condition);
            
            var musicianProperty = serializedObject.FindProperty("musician");
            musicianProperty.objectReferenceValue = actorCharacterRegistry.GetSafe((Actor)Enum.Parse(typeof(Actor), split[0]));
            
            var emotionProperty = serializedObject.FindProperty("emotion");
            emotionProperty.enumValueIndex = (int) Enum.Parse(typeof(Emotion), split[1]);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}