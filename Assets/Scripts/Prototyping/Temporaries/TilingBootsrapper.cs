using UnityEngine;

namespace Orion.Prototyping
{
    public class TilingBootsrapper : MonoBehaviour
    {
        [SerializeField] private PlayArea playArea;
        [SerializeField] private SomeTilable[] tilables;
        
        void Start()
        {
            playArea.Generate();
            foreach (var tilable in tilables) tilable.Place();
        }
    }
}