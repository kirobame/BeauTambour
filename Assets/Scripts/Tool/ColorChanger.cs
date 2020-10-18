using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Tooling
{
    public class ColorChanger : Executable
    {
        #region Encapsuled Types

        [HideReferenceObjectPicker]
        private class Pair
        {
            [HorizontalGroup("Horizontal"), HideLabel, SerializeField] private IColorable target;
            [HorizontalGroup("Horizontal"), HideLabel, SerializeField] private Color color;

            public void Execute() => target.SetColor(color);
        }

        #endregion
        
        [ListDrawerSettings(AlwaysAddDefaultValue = true)]
        [SerializeField] private Pair[] pairs = new Pair[0];
        
        public override void Execute() {  foreach (var pair in pairs) pair.Execute(); }
    }
}