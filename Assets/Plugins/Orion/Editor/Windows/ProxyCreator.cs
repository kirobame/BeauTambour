using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    public class ProxyCreator : OdinEditorWindow
    {
        [MenuItem("Tools/Orion/Proxy Creator")]
        static void OpenWindow() => GetWindow<ProxyCreator>().Show();

        public Type type;
        [FolderPath] public string destination;

        private const string templatePath = "Assets/Plugins/Orion/Templates";
        
        private bool CanButtonBeEnabled
        {
            get
            {
                if (type == null || destination == string.Empty) return false;
                
                var directory = $"{destination}/{type.Name}";
                return Directory.Exists(destination) && !Directory.Exists(directory);
            }
        }

        [Button, EnableIf("CanButtonBeEnabled")]
        private void Create()
        {
            AssetDatabase.CreateFolder(destination, type.Name);

            var hasNamespace = type.Namespace != string.Empty;

            var metaTemplate = new string[]
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
            
            var guids = AssetDatabase.FindAssets("t:TextAsset", new string[] {templatePath});
            var paths = new List<string>();
            
            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guid));
                var text = asset.text;

                if (hasNamespace)
                {
                    var namespaceEntry = $"using {type.Namespace};";
                    if (!text.Contains(namespaceEntry)) text = text.Insert(0, namespaceEntry + Environment.NewLine);
                }
                text = text.Replace("#Type#", type.Name);

                var nameStartIndex = text.IndexOf("class", StringComparison.Ordinal) + 6;
                var nameEndIndex = text.IndexOf(':', nameStartIndex) - 1;

                var name = text.Substring(nameStartIndex, nameEndIndex - nameStartIndex);

                var path = $"{destination}/{type.Name}/{name}.cs";
                File.WriteAllText(path, text);
                
                paths.Add(path);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            foreach (var path in paths)
            {
                var meta = new string[metaTemplate.Length];
                metaTemplate.CopyTo(meta,0);

                meta[1] += AssetDatabase.AssetPathToGUID(path);

                var icon = default(Texture);
                
                if (path.Contains("Listener")) icon = Resources.Load<Texture>("Icons/Types/UnityEngine.RaycastHit@32x");
                else if (path.Contains("Event")) icon = Resources.Load<Texture>("Icons/Types/UnityEngine.EventSystems.BaseEventData@32x");
                else icon = type.GetIcon();
                
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(icon, out var iconId, out long iconInstanceId);
                meta[7] += '{' + $"fileID: {iconInstanceId}, guid: {iconId}, type: 3" + '}';
                
                File.SetAttributes(path + ".meta", FileAttributes.Normal);
                File.WriteAllLines(path + ".meta", meta);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}