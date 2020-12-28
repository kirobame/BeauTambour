using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    public static class MenuItems
    {
        [MenuItem("Tools/Beau Tambour/Package AudioClips")]
        public static void PackageAudioClips()
        {
            var audioClips = Selection.GetFiltered<AudioClip>(SelectionMode.Assets);
            foreach (var audioClip in audioClips)
            {
                var path = AssetDatabase.GetAssetPath(audioClip);
                var lastSlashIndex = path.LastIndexOf('/');
                path = path.Remove(lastSlashIndex);
                
                var singleAudioPackage = ScriptableObject.CreateInstance<SingleAudioPackage>();
                var serializedAsset = new SerializedObject(singleAudioPackage);

                var clipProperty = serializedAsset.FindProperty("clip");
                clipProperty.objectReferenceValue = audioClip;

                serializedAsset.ApplyModifiedProperties();

                var creationPath = $"{path}/Packaged-{audioClip.name}.asset";
                AssetDatabase.CreateAsset(singleAudioPackage, creationPath);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}