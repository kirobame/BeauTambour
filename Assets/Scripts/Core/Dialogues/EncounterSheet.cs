using System;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class EncounterSheet : RuntimeSheet<Dialogue>
    {
        protected override Dialogue ProcessValue(string item)
        {
            item = item.Replace("-", string.Empty);
            var split = item.Split('[', ']').Where(subItem => subItem != string.Empty).ToArray();
            
            var cues = new Cue[split.Length / 2];
            var cueIndex = 0;
            
            for (var i = 0; i < cues.Length; i++)
            {
                var actor = (Actor)Enum.Parse(typeof(Actor), split[cueIndex]);
                var text = split[cueIndex + 1];
                
                cues[i] = new Cue(actor, text);
                
                cueIndex += 2;
            }
            
            return new Dialogue(cues);
        }
    }
}