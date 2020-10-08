using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Orion.Editor
{
    public class OrionEventDrawer<T> : OdinValueDrawer<T> where T : OrionEventBase
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Property.Children.First().Draw(label);
        }
    }
}