using System.Linq;
using Ludiq.OdinSerializer.Utilities;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    public class FeedbackDrawer<T> : OdinValueDrawer<T> where T : Feedback
    {
        private PropertyTree tree;
        private LocalPersistentContext<bool> foldout;

        protected override void Initialize()
        {
            base.Initialize();

            var target = Property.ValueEntry.WeakSmartValue as Object;
            tree = PropertyTree.Create(target);
            
            Property.Context.GetPersistent(this, $"{target.name}-Feedback-Foldout", out foldout);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var target = Property.ValueEntry.WeakSmartValue as Object;
            if (target != null)
            {
                foldout.Value = EditorGUILayout.Foldout(foldout.Value, new GUIContent(typeof(T).GetNiceName().Replace("Effect", string.Empty)));

                if (SirenixEditorGUI.BeginFadeGroup(this, foldout.Value))
                {
                    InspectorUtilities.BeginDrawPropertyTree(tree, true);
                    InspectorUtilities.DrawPropertiesInTree(tree);
                    InspectorUtilities.EndDrawPropertyTree(tree);
                }
                SirenixEditorGUI.EndFadeGroup();
            }
        }
    }
}