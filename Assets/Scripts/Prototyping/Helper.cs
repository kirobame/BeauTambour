using System;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Helper : SerializedMonoBehaviour
    {
        #region Encapsuled Types

        [Flags]
        private enum SomeType
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 4,
            Four = 8,
        }
        #endregion

        [SerializeField] private SomeType enumValue;

        [Button]
        void Check()
        {
            var firstValue = SomeType.One | SomeType.Four | SomeType.Three;
            var secondValue = SomeType.One | SomeType.Three;
            
            Debug.Log((firstValue & secondValue) == secondValue);
        }
    }
}