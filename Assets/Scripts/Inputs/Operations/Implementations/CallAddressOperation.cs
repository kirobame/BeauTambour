using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewCallAddressOperation", menuName = "Beau Tambour/Operations/Call Address")]
    public class CallAddressOperation : PhaseBoundOperation
    {
        [SerializeField] private EnumAddress address;
        
        protected override void RelayedOnStart(EventArgs inArgs)
        {
            Event.Call(address.Get());
        }
    }
}