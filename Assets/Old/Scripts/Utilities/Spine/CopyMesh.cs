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
        
        void LateUpdate()
        {
            if (copyTo == null || copyFrom == null)
                return;
            copyTo.sharedMesh = copyFrom.sharedMesh;
        }
    }
}