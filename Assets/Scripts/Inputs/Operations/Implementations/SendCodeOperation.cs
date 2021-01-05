using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSendCodeOperation", menuName = "Beau Tambour/Operations/Send Code")]
    public class SendCodeOperation : PhaseBoundOperation
    {
        [SerializeField] private EnumAddress address;
        [SerializeField] private string code;
        
        protected override void RelayedOnStart(EventArgs inArgs)
        {
            Event.Call<string>(address.Get(), code);
        }
    }
}