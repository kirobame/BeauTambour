using System;
using UnityEngine;

namespace Flux
{
    [Serializable]
    public struct EnumAddress
    {
        public Type Type => Type.GetType(stringedType);

        [SerializeField] private string stringedType;
        [SerializeField] private string name;
        
        public string Get()
        {
            if (name == string.Empty) return "None";
            else return ((Enum) Enum.Parse(Type, name)).GetNiceName();
        }
    }
}