﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Flux.Editor
{
    public class IconPostProcessor : AssetPostprocessor
    {
        private static readonly string[] metaTemplate = new string[]
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
        
        private static readonly (Type type, long id)[] lookups = new (Type type, long id)[]
        {
            (typeof(Listener), -7098612008054524421),
            (typeof(LocalListener), -7098612008054524421),
            (typeof(Sequencer), 3920131678736184407),
            (typeof(Poolable), -3633255215815038627),
            (typeof(Pool), 8714319771344428160),
            (typeof(PreProcessor), 943103792077196548),
            (typeof(InputHandler), 8653171293167480952),
            (typeof(InputActivator), 553123917656048335),
            (typeof(OperationHandler), 4058739119320582626),
            (typeof(Operation), 5586426421348142992)
        };

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var extendedLookUps = new List<(Type type, long id)>(lookups);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attribute = type.GetCustomAttribute<IconIndicatorAttribute>();
                    if (attribute != null) extendedLookUps.Add((type, attribute.IconId));
                }
            }
            
            foreach (var path in importedAssets)
            {
                if (!path.Contains("cs")) continue;

                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                var type = script.GetClass();

                if (type == null) continue;
                
                var attribute = type.GetCustomAttribute<IconProxyAttribute>();
                var hasAttribute = attribute != null;
                
                foreach (var lookup in extendedLookUps)
                {
                    if (hasAttribute) { if (!lookup.type.IsAssignableFrom(attribute.Target)) continue; }
                    else if (!lookup.type.IsAssignableFrom(type)) continue;

                    var meta = new string[metaTemplate.Length];
                    metaTemplate.CopyTo(meta,0);
                
                    meta[1] += AssetDatabase.AssetPathToGUID(path);
                    meta[7] += '{' + $"fileID: {lookup.id}, guid: 0000000000000000d000000000000000, type: 0" + '}';

                    var metaPath = $"{path}.meta";
                    File.SetAttributes(metaPath, FileAttributes.Normal);
                    File.WriteAllLines(metaPath, meta);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}