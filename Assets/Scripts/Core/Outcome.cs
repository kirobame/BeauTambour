using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class Outcome : ScriptableObject
    {
        public bool IsOperational => conditions.All(condition => condition.IsMet());
        public int Priority => priority;
        
        [SerializeField] private int priority;
        [SerializeField] private Condition[] conditions;
        [SerializeField] private Sequencer sequencerPrefab;

        public Sequencer Sequencer { get; private set; }

        public void BootUp()
        {
            if (!Application.isPlaying || Sequencer != null) return;
            
            var parent = Repository.GetSingle<Transform>(Reference.OutcomesParent);
            Sequencer = Instantiate(Sequencer, parent);
        }
    }
}