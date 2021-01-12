using System.Collections.Generic;
using System.Linq;
using Deprecated;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-976224214725393938), CreateAssetMenu(fileName = "NewSignalCollection", menuName = "Beau Tambour/General/Collections/Signal")]
    public class SignalCollection : Collection<Signal>
    {
        private Dictionary<string,  Signal[]> registry;
        
        public void BootUp()
        {
            registry = new Dictionary<string, Signal[]>();
            foreach (var group in values.GroupBy(value => value.Category))
            {
                var signals = new Signal[Extensions.GetEnumCount<Emotion>()];
                foreach (var signal in group)
                {
                    signal.Bootup();
                    signals[(int)signal.Key] = signal;
                }
                
                registry.Add(group.Key, signals);
            }
        }

        public bool TrySelect(string category, Emotion emotion, out Signal signal)
        {
            if (registry.TryGetValue(category, out var signals))
            {
                signal = signals[(int)emotion];
                return true;
            }
            else
            {
                signal = null;
                return false;
            }
        }
    }
}