using System;
using Ludiq.OdinSerializer.Utilities;
using UnityEditor;
using UnityEngine;

namespace Deprecated.Editor
{
    public class BeauTambourPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets.Append(movedAssets).Append(movedFromAssetPaths))
            {
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if (!(asset is Outcome outcome)) continue;

                BeauTambourUtilities.ModifyPathFor(outcome);
            }

            foreach (var path in deletedAssets)
            {
                if (!BeauTambourUtilities.Outcomes.TryGetValue(path, out var prefabId)) continue;
                AssetDatabase.DeleteAsset($"Assets/Prefabs/Outcomes/{prefabId}.prefab");
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}