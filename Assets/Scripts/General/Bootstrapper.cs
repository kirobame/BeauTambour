using System.Collections;
using System.Collections.Generic;
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
        void Start() => StartCoroutine(StartRoutine());
        
        private IEnumerator StartRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            encounter.Bootup(this, useBackup);
        }
    }
}