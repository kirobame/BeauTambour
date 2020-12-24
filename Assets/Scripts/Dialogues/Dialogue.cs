using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Deprecated;
using UnityEngine;

namespace BeauTambour
{
    public class Dialogue
    {
        public static Dialogue Parse(string value)
        {
            var split = value.Split(new char[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries);

            var cues = new List<Cue>();
            for (var i = 0; i < split.Length; i += 2)
            {
                var actor = (Actor)Enum.Parse(typeof(Actor), split[i]);

                var body = split[i + 1];
                
                var index = body.IndexOf('-');
                if (index != -1) body = body.Remove(index, 1);
                
                var texts = body.Split(new string[] {"//"}, StringSplitOptions.None);
                foreach (var text in texts)
                {
                    var match = Regex.Match(text, "\r\n|\r|\n");
                    if (match.Index == 0)
                    {
                        var trimmedText = text.Remove(0, match.Length);
                        cues.Add(new Cue(actor, trimmedText));
                    }
                    else cues.Add(new Cue(actor, text));
                }
            }

            return new Dialogue(cues.ToArray());
        }
        
        public Dialogue(Cue[] cues) => this.cues = cues;

        public Cue this[int index] => cues[index];
        public int Length => cues.Length;

        public IReadOnlyList<Cue> Cues => cues;
        private Cue[] cues;

        public Dialogue Trim(Vector2Int range)
        {
            var trimmedCues = new Cue[range.y - range.x + 1];

            var index = 0;
            for (var i = range.x; i <= range.y; i++)
            {
                trimmedCues[index] = cues[i];
                index++;
            }
            
            return new Dialogue(trimmedCues);
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var cue in cues) builder.Append(cue.ToString());

            return builder.ToString();
        }
    }
}