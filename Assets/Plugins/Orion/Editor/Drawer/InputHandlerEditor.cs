using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    [CustomEditor(typeof(InputHandler))]
    public class InputHandlerEditor : OdinEditor
    {
        private const float maxButtonWidth = 175f;
        
        public override void OnInspectorGUI()
        {
            var tree = Tree;
            var inputHandler = target as InputHandler;
            
            InspectorUtilities.BeginDrawPropertyTree(tree, true);   
            
            var sourceProperty = tree.GetPropertyAtPath("inputSource"); 
            sourceProperty.Draw();

            var actionMaps = inputHandler.Source.actionMaps;
            var mapsProperty = tree.GetPropertyAtPath("activeMaps");
            var activeMaps = mapsProperty.ValueEntry.WeakSmartValue as List<int>;
            
            var columns = Mathf.RoundToInt((EditorGUIUtility.currentViewWidth - 40f) / maxButtonWidth);
            var count = 0f;
            
            EditorGUILayout.BeginHorizontal();
            for (var i = 0; i < actionMaps.Count; i++)
            {
                if (count == columns)
                {
                    columns = 0;
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.BeginHorizontal();
                }
                
                if (activeMaps.Contains(i))
                {
                    if (GUILayout.Button(actionMaps[i].name)) activeMaps.Remove(i);
                }
                else
                {
                    GUIHelper.PushColor(new Color(0.6f,0.6f,0.6f, 0.75f));
                    if (GUILayout.Button(actionMaps[i].name)) activeMaps.Add(i);
                    
                    GUIHelper.PopColor();
                }

                count++;
            }
            EditorGUILayout.EndHorizontal();
            mapsProperty.ValueEntry.WeakSmartValue = activeMaps;
            
            var linksProperty = tree.GetPropertyAtPath("inputActionLinks"); 
            linksProperty.Draw();
            
            InspectorUtilities.EndDrawPropertyTree(tree);
        }
    }
}