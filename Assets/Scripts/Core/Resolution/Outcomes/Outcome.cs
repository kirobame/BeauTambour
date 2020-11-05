using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(1891504476101499824), CreateAssetMenu(fileName = "NewOutcome", menuName = "Beau Tambour/Resolution/Outcome")]
    public class Outcome : ScriptableObject
    {
        public bool IsOperational => !conditions.Any() || conditions.All(condition => condition.IsMet());
        //
        public bool AllowsSuccession => allowsSuccession;
        public int Priority => priority;

        [SerializeField] private bool allowsSuccession;
        [SerializeField] private int priority;
        
        [SerializeField] private Condition[] conditions;
        [SerializeField] private Sequencer sequencerPrefab;

        public Sequencer Sequencer { get; private set; }

        public void BootUp()
        {
            if (!Application.isPlaying || Sequencer != null) return;
            
            var parent = Repository.GetSingle<Transform>(Parent.Outcomes);
            Sequencer = Instantiate(sequencerPrefab, parent);
        }

        public void Play(Note[] notes) => Sequencer.Play();
    }
}