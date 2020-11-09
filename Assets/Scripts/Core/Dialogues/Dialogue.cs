using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace BeauTambour
{
    public class Dialogue
    {
        public static Dialogue Parse(string value)
        {
            value = value.Replace("-", string.Empty);
            var split = value.Split('[', ']').Where(subItem => subItem != string.Empty).ToList();
            
            var cues = new Cue[split.Count / 2];
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
        
        public Dialogue(Cue[] cues) => this.cues = cues;

        public IReadOnlyList<Cue> Cues => cues;
        private Cue[] cues;
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var cue in cues) builder.Append(cue.ToString());

            return builder.ToString();
        }
    }
}