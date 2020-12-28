using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Encounter encounter;
        [SerializeField] private bool useBackup;

        void Awake() => GameState.Bootup();
        void Start() => encounter.Bootup(this, useBackup);
    }
}