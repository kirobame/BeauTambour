using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [InlineProperty]
    public class AssetProvider<T> : IProvider<T> where T : Object
    {
        [SerializeField, HideLabel, AssetsOnly] private T prefab;
        
        object IProvider.GetInstance() => GetInstance();
        public T GetInstance() => Object.Instantiate(prefab);
    }
}