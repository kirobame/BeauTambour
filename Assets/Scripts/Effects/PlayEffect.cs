using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [ItemPath("General/Sequencer/Play")]
    [ItemName("Play Sequence")]
    public class PlayEffect : Effect
    {
        [SerializeField] private Sequencer target;

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            target.Play();
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}