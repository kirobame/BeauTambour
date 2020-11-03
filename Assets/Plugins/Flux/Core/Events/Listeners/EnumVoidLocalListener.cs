using UnityEngine;
using UnityEngine.Events;

namespace Flux
{
    public class EnumVoidLocalListener : LocalVoidListener
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
}