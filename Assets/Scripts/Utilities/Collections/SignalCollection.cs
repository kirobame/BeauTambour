using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-976224214725393938), CreateAssetMenu(fileName = "NewSignalCollection", menuName = "Beau Tambour/General/Collections/Signal")]
    public class SignalCollection : Collection<Signal>
    {
        private Dictionary<string, Dictionary<Emotion, List<Signal>[]>> registry;
        
        public void Initialize()
        {
            registry = new Dictionary<string, Dictionary<Emotion, List<Signal>[]>>();
            foreach (var group in values.GroupBy(value => value.Category))
            {
                Debug.Log($"Categories -> {group.Key}");
                registry.Add(group.Key, new Dictionary<Emotion, List<Signal>[]>());
                
                foreach (var subGroup in group.GroupBy(signal => signal.Key))
                {
                    registry[group.Key].Add(subGroup.Key, new List<Signal>[3]);
                    for (var i = 0; i < 3; i++) registry[group.Key][subGroup.Key][i] = new List<Signal>();

                    foreach (var signal in subGroup)
                    {
                        Debug.Log("Signal : " + signal.name);
                        registry[group.Key][subGroup.Key][signal.Clarity].Add(signal);
                    }
                }
            }
        }

        public Signal Select(string category, Emotion emotion, int clarity)
        {
            Debug.Log("Gotten category : " + category);
            
            var runtimeSettings = Repository.GetSingle<RuntimeSettings>(Reference.RuntimeSettings);
            clarity = runtimeSettings.GlobalClarity > clarity ? runtimeSettings.GlobalClarity : clarity;

            var index = Random.Range(0, registry[category][emotion][clarity].Count);
            return registry[category][emotion][clarity][index];
        }
    }
}