using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Orion.Editor
{
    public class IconSetter : OdinEditorWindow
    {
        [MenuItem("Tools/Orion/Icon Setter")]
        static void OpenWindow() => GetWindow<IconSetter>().Show();
        
        public MonoScript script;
        public long iconId;

        private bool CanButtonBeEnabled => script != null;
        
        [Button, EnableIf("CanButtonBeEnabled")]
        private void Execute()
        {
            var path = AssetDatabase.GetAssetPath(script);
            var meta = new string[]
            {
                "fileFormatVersion: 2",
                "guid: ",
                "MonoImporter:",
                "  externalObjects: {}",
                "  serializedVersion: 2",
                "  defaultReferences: []",
                "  executionOrder: 0",
                "  icon: ",
                "  userData: ",
                "  assetBundleName: ",
                "  assetBundleVariant: ",
            };

            meta[1] += AssetDatabase.AssetPathToGUID(path);
            meta[7] += '{' + $"fileID: {iconId}, guid: 0000000000000000d000000000000000, type: 0" + '}';

            var metaPath = path + ".meta";
            File.SetAttributes(metaPath, FileAttributes.Normal);
            File.WriteAllLines(metaPath, meta);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}