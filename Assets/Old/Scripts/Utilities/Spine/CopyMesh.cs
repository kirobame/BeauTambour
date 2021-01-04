using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Spine
{
    [ExecuteAlways]
    public class CopyMesh : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter copyFrom = default;
        [SerializeField]
        private MeshFilter copyTo = default;
        
        public MeshFilter CopyFrom
        {
            set
            {
                if (value == null) return;
                copyFrom = value;
            }
        }

        void LateUpdate()
        {
            if (copyTo == null || copyFrom == null)
                return;
            copyTo.sharedMesh = copyFrom.sharedMesh;
        }
    }
}