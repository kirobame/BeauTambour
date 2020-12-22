using TMPro;
using UnityEngine;

namespace Deprecated
{
    public struct Cue 
    {
        public Cue(Actor actor, string text)
        {
            var occurence = text.IndexOf('(');
            while (occurence != -1)
            {
                if (occurence > 0 && text[occurence - 1] == ' ') occurence--;
                
                var end = text.IndexOf(')', occurence) + 1;
                if (text[end] == ' ') end++;

                text = text.Remove(occurence, end - occurence);
                occurence = text.IndexOf('(');
            }
            
            Actor = actor;
            Text = text;
        }
        
        public readonly Actor Actor;
        public readonly string Text;

        public override string ToString() => $"[{Actor}]-{Text}";
    }
}