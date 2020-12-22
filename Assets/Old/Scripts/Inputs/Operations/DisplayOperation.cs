using System;
using Flux;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewDisplayOperation", menuName = "Beau Tambour/Operations/Display")]
    public class DisplayOperation : SingleOperation
    {
        [SerializeField] private string message;
        
        public override void OnStart(EventArgs inArgs) => Debug.Log(message);
    }
}