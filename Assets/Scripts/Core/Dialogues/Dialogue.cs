using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace BeauTambour
{
    public class Dialogue
    {
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