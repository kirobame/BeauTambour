using Flux.Editor;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CustomEditor(typeof(PoolableAudio))]
    public class PoolableAudioEditor : MonoBehaviourEditor
    {
        protected override void Draw(SerializedProperty iterator)
        {
            iterator.NextVisible(false);
            EditorGUILayout.PropertyField(iterator);

            var audioSource = (AudioSource)iterator.objectReferenceValue;
            if (audioSource == null) GUI.enabled = false;

            if (GUILayout.Button("Test Audio") && audioSource != null) audioSource.Play();
            GUI.enabled = true;
        }
    }
}